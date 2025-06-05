using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class ICloudLoaderPopup : GUIPopup
{
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnRestoreProgressAgreed;

	private const string COUNT_PREFIX = "/";

	[SerializeField]
	private tk2dTextMesh[] loadCoinsLabels;

	[SerializeField]
	private tk2dTextMesh[] loadGemsLabels;

	[SerializeField]
	private tk2dTextMesh[] loadArchersLabels;

	[SerializeField]
	private tk2dUIItem[] loadButtons;

	[SerializeField]
	private tk2dTextMesh[] uploadCoinsLabels;

	[SerializeField]
	private tk2dTextMesh[] uploadGemsLabels;

	[SerializeField]
	private tk2dTextMesh[] uploadArchersLabels;

	[SerializeField]
	private tk2dUIItem[] uploadButtons;

	[SerializeField]
	private TweenPosition[] showTweeners;

	[SerializeField]
	private tk2dUIItem closeButtonAdditional;

	private ICloudManager cachedICloudManager;

	private int savedCharacters;

	private int savedCoins;

	private int savedGems;

	private int cachedDefaultCharactersCount;

	private bool isAllCharactersBoughtInData;

	private ICloudManager CachedICloudManager
	{
		get
		{
			if (cachedICloudManager == null)
			{
				cachedICloudManager = SingletonMonoBehaviour<ICloudManager>.Instance;
			}
			return cachedICloudManager;
		}
	}

	private int CurrentCoins
	{
		get
		{
			return base.CachedManagers.CachedStatisticsManager.Coins;
		}
	}

	private int CurrentGems
	{
		get
		{
			return base.CachedManagers.CachedStatisticsManager.Gems;
		}
	}

	private int CurrentCharacters
	{
		get
		{
			return (!base.CachedManagers.CachedShopManager.AllCharactersBought) ? (base.CachedManagers.CachedShopManager.BoughtCharacters.Count + base.CachedManagers.CachedShopManager.DefaultCharacters.Count) : base.CachedManagers.CachedShopManager.BoughtCharacters.Count;
		}
	}

	private int SavedCharacters
	{
		get
		{
			return (!base.CachedManagers.CachedShopManager.AllCharactersBought && !IsAllCharactersBoughtInData) ? (savedCharacters + base.CachedManagers.CachedShopManager.DefaultCharacters.Count) : savedCharacters;
		}
	}

	private bool IsLoadDataProfitable
	{
		get
		{
			return SavedCharacters > CurrentCharacters || (SavedCharacters == CurrentCharacters && savedGems > CurrentGems) || (SavedCharacters == CurrentCharacters && savedGems == CurrentGems && savedCoins > CurrentCoins);
		}
	}

	private bool IsAllCharactersBoughtInData
	{
		get
		{
			return isAllCharactersBoughtInData;
		}
		set
		{
			if (!isAllCharactersBoughtInData)
			{
				isAllCharactersBoughtInData = value;
			}
		}
	}

	public static event Action OnRestoreProgressAgreed
	{
		add
		{
			Action action = ICloudLoaderPopup.m_OnRestoreProgressAgreed;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref ICloudLoaderPopup.m_OnRestoreProgressAgreed, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = ICloudLoaderPopup.m_OnRestoreProgressAgreed;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref ICloudLoaderPopup.m_OnRestoreProgressAgreed, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		UpdateInfo(CachedICloudManager.AutosavedCoins, CachedICloudManager.AutosavedCharacters, CachedICloudManager.AutosavedGems, CachedICloudManager.AutosavedAllIncludedBought);
		CachedICloudManager.ReadProgressFromICloud(false);
		if (closeButtonAdditional != null)
		{
			closeButtonAdditional.OnClick += CloseButton_OnClick;
		}
		for (int i = 0; i < loadButtons.Length; i++)
		{
			loadButtons[i].OnClick += LoadButton_OnClick;
		}
		for (int j = 0; j < uploadButtons.Length; j++)
		{
			uploadButtons[j].OnClick += UploadButton_OnClick;
		}
		ICloudManager.OnProgressDownloaded += ICloudManager_OnProgressDownloaded;
		ICloudManager.OnProgressReaded += ICloudManager_OnProgressReaded;
		WarningRestoreProgress.OnButtonClicked += WarningRestoreProgress_OnButtonClicked;
		if (!CachedICloudManager.IsStorePopupInSettingsShowed)
		{
			base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.StoreWarning, true, null, true);
			CachedICloudManager.IsStorePopupInSettingsShowed = true;
		}
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		if (closeButtonAdditional != null)
		{
			closeButtonAdditional.OnClick -= CloseButton_OnClick;
		}
		for (int i = 0; i < loadButtons.Length; i++)
		{
			loadButtons[i].OnClick -= LoadButton_OnClick;
		}
		for (int j = 0; j < uploadButtons.Length; j++)
		{
			uploadButtons[j].OnClick -= UploadButton_OnClick;
		}
		ICloudManager.OnProgressDownloaded -= ICloudManager_OnProgressDownloaded;
		ICloudManager.OnProgressReaded -= ICloudManager_OnProgressReaded;
		WarningRestoreProgress.OnButtonClicked -= WarningRestoreProgress_OnButtonClicked;
	}

	protected override void Update()
	{
		base.Update();
		for (int i = 0; i < showTweeners.Length; i++)
		{
			if (!showTweeners[i].gameObject.activeInHierarchy && showTweeners[i].enabled)
			{
				showTweeners[i].Update();
			}
		}
	}

	private void UpdateInfo(int coins, int characters, int gems, bool isAllCharactersBought)
	{
		savedCoins = coins;
		savedGems = gems;
		savedCharacters = characters;
		isAllCharactersBoughtInData = isAllCharactersBought;
		for (int i = 0; i < loadCoinsLabels.Length; i++)
		{
			loadCoinsLabels[i].text = savedCoins.ToString();
		}
		for (int j = 0; j < loadArchersLabels.Length; j++)
		{
			loadArchersLabels[j].text = SavedCharacters + "/" + base.CachedManagers.CachedShopManager.AvailableCharacters.Count;
		}
		for (int k = 0; k < loadGemsLabels.Length; k++)
		{
			loadGemsLabels[k].text = savedGems.ToString();
		}
		for (int l = 0; l < uploadCoinsLabels.Length; l++)
		{
			uploadCoinsLabels[l].text = CurrentCoins.ToString();
		}
		for (int m = 0; m < uploadArchersLabels.Length; m++)
		{
			uploadArchersLabels[m].text = CurrentCharacters + "/" + base.CachedManagers.CachedShopManager.AvailableCharacters.Count;
		}
		for (int n = 0; n < uploadGemsLabels.Length; n++)
		{
			uploadGemsLabels[n].text = CurrentGems.ToString();
		}
	}

	private void LoadButton_OnClick()
	{
		if (!CachedICloudManager.IsSavingDataAvailable)
		{
			return;
		}
		if (IsLoadDataProfitable)
		{
			ICloudLoaderPopup.m_OnRestoreProgressAgreed?.Invoke();
		}
		else
		{
			base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.WarningRestoreProgress, true, null, true);
			CachedICloudManager.BadChoiceType = ChoiceType.Load;
		}
	}

	private void UploadButton_OnClick()
	{
		if (CachedICloudManager.IsSavingDataAvailable)
		{
			if (!IsLoadDataProfitable)
			{
				ICLoudSaveType iCloudStore = base.CachedManagers.CachedStatisticsManager.ICloudStore;
				base.CachedManagers.CachedStatisticsManager.ICloudStore = ICLoudSaveType.NeededAndRestored;
				CachedICloudManager.SaveProgressToICloud();
				base.CachedManagers.CachedStatisticsManager.ICloudStore = iCloudStore;
			}
			else
			{
				base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.WarningRestoreProgress, true, null, true);
				CachedICloudManager.BadChoiceType = ChoiceType.Upload;
			}
		}
	}

	private void ICloudManager_OnProgressReaded(int coins, int characters, int gems, bool isAllCharactersBought)
	{
		UpdateInfo(coins, characters, gems, isAllCharactersBought);
	}

	private void ICloudManager_OnProgressDownloaded()
	{
		UpdateInfo(CachedICloudManager.AutosavedCoins, CachedICloudManager.AutosavedCharacters, CachedICloudManager.AutosavedGems, CachedICloudManager.AutosavedAllIncludedBought);
	}

	private void WarningRestoreProgress_OnButtonClicked(bool isAproved)
	{
		if (isAproved)
		{
			if (CachedICloudManager.BadChoiceType == ChoiceType.Load && ICloudLoaderPopup.m_OnRestoreProgressAgreed != null)
			{
				ICloudLoaderPopup.m_OnRestoreProgressAgreed();
			}
			else if (CachedICloudManager.BadChoiceType == ChoiceType.Upload)
			{
				ICLoudSaveType iCloudStore = base.CachedManagers.CachedStatisticsManager.ICloudStore;
				base.CachedManagers.CachedStatisticsManager.ICloudStore = ICLoudSaveType.NeededAndRestored;
				CachedICloudManager.SaveProgressToICloud();
				base.CachedManagers.CachedStatisticsManager.ICloudStore = iCloudStore;
			}
		}
		CachedICloudManager.BadChoiceType = ChoiceType.None;
	}
}
