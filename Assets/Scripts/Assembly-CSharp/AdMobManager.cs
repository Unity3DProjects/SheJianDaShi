using System.Collections;
using UnityEngine;

public class AdMobManager : SingletonMonoBehaviour<AdMobManager>
{
	private const string APPSFLYER_ADMOB_AD_EVENT = "af_admob_load";

	private const string TENJIN_ADMOB_AD_EVENT = "tj_admob_load";

	private const string ADMOB_LOAD_COUNT = "admob_load_count";

	private const string ADMOB_IMPRESSION_REVENUE_LINK = "https://s3.eu-central-1.amazonaws.com/game-data-revenue/archer_revenue.txt";

	private const float ADMOB_IMPRESSION_DEFAULT_REVENUE = 0.00018f;

	private bool isBannerActive;

	private bool isBannerLocked;

	private Managers cachedManagers;

	private float receivedAdMobImpressionRevenue = -1f;

	private Managers CachedManagers
	{
		get
		{
			if (cachedManagers == null)
			{
				cachedManagers = SingletonMonoBehaviour<Managers>.Instance;
			}
			return cachedManagers;
		}
	}

	private bool IsBannerLocked
	{
		get
		{
			return isBannerLocked;
		}
		set
		{
			if (isBannerLocked != value)
			{
				isBannerLocked = value;
				if (isBannerLocked && IsBannerActive)
				{
					LLAdmobManager.HideBanner();
				}
				else if (!isBannerLocked && IsBannerAvailable && CachedManagers.CachedGUIScreenManager.ActiveScreen == GUIScreenType.Game)
				{
					ShowBanner();
				}
			}
		}
	}

	private bool IsBannerActive
	{
		get
		{
			return MCMoPubManager.Instance.IsBannerCached();
		}
		set
		{
			if (IsBannerAvailable && value)
			{
				MCMoPubManager.Instance.CreateBanner();
			}
			else
			{
				MCMoPubManager.Instance.DestroyBanner();
			}
		}
	}

	private bool IsBannerAvailable
	{
		get
		{
			bool flag = true;
			StatisticsManager cachedStatisticsManager = CachedManagers.CachedStatisticsManager;
			GameType currentGameType = CachedManagers.CachedGameManager.CurrentGameType;
			if ((currentGameType == GameType.VSComputer && cachedStatisticsManager.MathesCount <= 1) || (currentGameType == GameType.VSHuman && cachedStatisticsManager.MatchesCountVSHuman == 0) || (currentGameType == GameType.BirdsHunt && cachedStatisticsManager.BirdsKilled == 0) || (currentGameType == GameType.AppleShoot && cachedStatisticsManager.FruitsSmashed == 0) || (currentGameType == GameType.Multiplayer && cachedStatisticsManager.MathesCount == 0) || IsBannerLocked)
			{
				flag = false;
			}
			flag &= cachedStatisticsManager.SpentBucks == 0;
			return flag & (CachedManagers.CachedStatisticsManager.TotalSeesionCount > 0);
		}
	}

	public int AdmobLoadCount
	{
		get
		{
			return CustomPlayerPrefs.GetInt("admob_load_count", 0);
		}
		set
		{
			CustomPlayerPrefs.SetInt("admob_load_count", value);
			SendLoadCountEvents(value);
		}
	}

	private void OnEnable()
	{
		if (IsBannerAvailable)
		{
			GameManager.OnGameStart += GameManager_OnGameStart;
			GameManager.OnGameStop += GameManager_OnGameStop;
			GameManager.OnGamePause += GameManager_OnGamePause;
			GUIOrientationManger.OnDeviceOrientationChanged += GUIOrientationManger_OnDeviceOrientationChanged;
			GameManager.OnGamePause += GameManager_OnGamePaused;
			Tutorial.OnTutorialWillAppear += Tutorial_OnTutorialWillAppear;
			Tutorial.OnTutorialClose += Tutorial_OnTutorialClose;
			LLAdmobManager.OnBannerLoad += LLAdmobManager_OnBannerLoad;
			CustomGUI.OnDebugGUI += CustomGUI_OnDebugGUI;
		}
	}

	private void OnDisable()
	{
		if (IsBannerAvailable)
		{
			GameManager.OnGameStart -= GameManager_OnGameStart;
			GameManager.OnGameStop -= GameManager_OnGameStop;
			GameManager.OnGamePause -= GameManager_OnGamePause;
			GUIOrientationManger.OnDeviceOrientationChanged += GUIOrientationManger_OnDeviceOrientationChanged;
			GameManager.OnGamePause -= GameManager_OnGamePaused;
			Tutorial.OnTutorialWillAppear -= Tutorial_OnTutorialWillAppear;
			Tutorial.OnTutorialClose -= Tutorial_OnTutorialClose;
			LLAdmobManager.OnBannerLoad -= LLAdmobManager_OnBannerLoad;
			CustomGUI.OnDebugGUI -= CustomGUI_OnDebugGUI;
		}
	}

	private void TryInitialize()
	{
		LLAdmobManager.Initialize();
	}

	private void ShowBanner()
	{
		MCMoPubManager.Instance.SetBannerVisibility(true);
	}

	private void SendLoadCountEvents(int loadCount)
	{
	}

	private void GameManager_OnGameStart()
	{
		IsBannerActive = IsBannerAvailable;
	}

	private void GameManager_OnGameStop()
	{
		IsBannerActive = false;
	}

	private void GUIOrientationManger_OnDeviceOrientationChanged(GUILayouterRotationType rotation)
	{
	}

	private void Tutorial_OnTutorialClose()
	{
		ShowBanner();
	}

	private void Tutorial_OnTutorialWillAppear()
	{
		MCMoPubManager.Instance.SetBannerVisibility(false);
	}

	private void GameManager_OnGamePaused(bool paused)
	{
		if (paused)
		{
			Tutorial_OnTutorialWillAppear();
		}
		else
		{
			Tutorial_OnTutorialClose();
		}
	}

	private void LLAdmobManager_OnBannerLoad(bool result)
	{
	}

	private void GameManager_OnGamePause(bool isPaused)
	{
		if (isPaused)
		{
			LLAdmobManager.HideBanner();
		}
		else
		{
			ShowBanner();
		}
	}

	private IEnumerator RecreateBanner()
	{
		yield return new WaitForSeconds(1f);
		if (CachedManagers.CachedGUIScreenManager.ActiveScreen == GUIScreenType.Game)
		{
			LLAdmobManager.ShowBanner();
		}
	}

	private IEnumerator LoadAdMobRevenueFromServer()
	{
		WWW downloader = new WWW("https://s3.eu-central-1.amazonaws.com/game-data-revenue/archer_revenue.txt");
		yield return downloader;
		if (downloader.error != null)
		{
			CustomDebug.Log("Fail to download ad priority with error " + downloader.error);
		}
		else if (!float.TryParse(downloader.text, out receivedAdMobImpressionRevenue))
		{
			receivedAdMobImpressionRevenue = -1f;
		}
	}

	private void CustomGUI_OnDebugGUI()
	{
		if (GUI.Button(new Rect(Screen.width / 2, 0f, 100f, 50f), "UnlockBanners"))
		{
			IsBannerLocked = false;
		}
		if (GUI.Button(new Rect(Screen.width / 2 - 100, 0f, 100f, 50f), "LockBanners"))
		{
			IsBannerLocked = true;
		}
	}
}
