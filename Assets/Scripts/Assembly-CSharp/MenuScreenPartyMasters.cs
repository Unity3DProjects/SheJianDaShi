using System.Collections;
using System.Collections.Generic;
using Spine;
using UnityEngine;

public class MenuScreenPartyMasters : GUIScreen
{
	private static string RAPER_IDLE_HANDS_ANIMATION = "idle_money_hands";

	private static string RAPER_IDLE_ANIMATION = "idle_money";

	private static string RAPER_WIN_ANIMATION = "win";

	private static string GIRL_TWERK_ANIMATION = "twerk";

	private static string GIRL_JUMP_ANIMATION = "jump";

	[SerializeField]
	private SkeletonAnimation[] girlsAnimation;

	[SerializeField]
	private tk2dProgressBar[] girlsProgressBar;

	[SerializeField]
	private GUIControl[] boomboxControls;

	[SerializeField]
	private GUIControl[] proposalsControls;

	[SerializeField]
	private GUIControl[] flagsControls;

	[SerializeField]
	private GUIControl[] promoButtonsControls;

	[SerializeField]
	private tk2dUIItem[] hitButtons;

	[SerializeField]
	[ResourceLink]
	private AssetLink clothEffect;

	[SerializeField]
	[ResourceLink]
	private AssetLink moneyEffect;

	[SerializeField]
	private SkeletonAnimation[] tapsAnimation;

	private PartyGameStateType partyGameState;

	private MenuPhase currentPhase;

	private CharacterNameType currentCharacter;

	private float girlsHealt;

	private int tapCount;

	private bool isTapAnimationActive;

	private List<SkeletonAnimation> avatars = new List<SkeletonAnimation>();

	private Coroutine CoroutineChangeSkin;

	private Coroutine CoroutineParty;

	private bool IsTapAnimationActive
	{
		get
		{
			return isTapAnimationActive;
		}
		set
		{
			if (isTapAnimationActive != value)
			{
				isTapAnimationActive = value;
				SkeletonAnimation[] array = tapsAnimation;
				foreach (SkeletonAnimation skeletonAnimation in array)
				{
					skeletonAnimation.gameObject.SetActive(value);
				}
			}
		}
	}

	private PartyGameStateType PartyGameState
	{
		get
		{
			return partyGameState;
		}
		set
		{
			if (partyGameState == value)
			{
				return;
			}
			partyGameState = value;
			switch (value)
			{
			case PartyGameStateType.None:
			{
				GUIControl[] array11 = boomboxControls;
				foreach (GUIControl gUIControl5 in array11)
				{
					gUIControl5.Hide();
				}
				GUIControl[] array12 = proposalsControls;
				foreach (GUIControl gUIControl6 in array12)
				{
					gUIControl6.Show();
				}
				GUIControl[] array13 = flagsControls;
				foreach (GUIControl gUIControl7 in array13)
				{
					gUIControl7.Show();
				}
				GUIControl[] array14 = promoButtonsControls;
				foreach (GUIControl gUIControl8 in array14)
				{
					gUIControl8.HideImmediately();
				}
				tk2dUIItem[] array15 = hitButtons;
				foreach (tk2dUIItem tk2dUIItem4 in array15)
				{
					tk2dUIItem4.gameObject.SetActive(false);
				}
				tapCount = 0;
				IsTapAnimationActive = false;
				skinClear();
				AvatarOnDisable();
				break;
			}
			case PartyGameStateType.Active:
			{
				girlsHealt = 1f;
				tk2dProgressBar[] array4 = girlsProgressBar;
				foreach (tk2dProgressBar tk2dProgressBar3 in array4)
				{
					tk2dProgressBar3.fillAmount = girlsHealt;
				}
				GUIControl[] array5 = boomboxControls;
				foreach (GUIControl gUIControl in array5)
				{
					gUIControl.Show();
				}
				GUIControl[] array6 = promoButtonsControls;
				foreach (GUIControl gUIControl2 in array6)
				{
					gUIControl2.Show();
				}
				GUIControl[] array7 = proposalsControls;
				foreach (GUIControl gUIControl3 in array7)
				{
					gUIControl3.Hide();
				}
				GUIControl[] array8 = flagsControls;
				foreach (GUIControl gUIControl4 in array8)
				{
					gUIControl4.Hide();
				}
				tk2dUIItem[] array9 = hitButtons;
				foreach (tk2dUIItem tk2dUIItem3 in array9)
				{
					tk2dUIItem3.gameObject.SetActive(true);
				}
				SkeletonAnimation[] array10 = girlsAnimation;
				foreach (SkeletonAnimation skeletonAnimation2 in array10)
				{
					skeletonAnimation2.Reset();
					skeletonAnimation2.AnimationName = GIRL_TWERK_ANIMATION;
				}
				foreach (SkeletonAnimation avatar in avatars)
				{
					avatar.State.SetAnimation(0, RAPER_IDLE_ANIMATION, true);
				}
				AvatarOnEnable();
				break;
			}
			case PartyGameStateType.Finish:
			{
				tk2dUIItem[] array = hitButtons;
				foreach (tk2dUIItem tk2dUIItem2 in array)
				{
					tk2dUIItem2.gameObject.SetActive(false);
				}
				girlsHealt = 0f;
				tk2dProgressBar[] array2 = girlsProgressBar;
				foreach (tk2dProgressBar tk2dProgressBar2 in array2)
				{
					tk2dProgressBar2.fillAmount = girlsHealt;
				}
				SkeletonAnimation[] array3 = girlsAnimation;
				foreach (SkeletonAnimation skeletonAnimation in array3)
				{
					skeletonAnimation.Reset();
					skeletonAnimation.AnimationName = GIRL_JUMP_ANIMATION;
				}
				{
					foreach (SkeletonAnimation avatar2 in avatars)
					{
						avatar2.State.SetAnimation(0, RAPER_WIN_ANIMATION, true);
						avatar2.State.SetAnimation(1, RAPER_WIN_ANIMATION, true);
					}
					break;
				}
			}
			}
		}
	}

	private void Start()
	{
		GUIControl[] array = proposalsControls;
		foreach (GUIControl gUIControl in array)
		{
			gUIControl.ShowImmediately();
		}
		GUIControl[] array2 = flagsControls;
		foreach (GUIControl gUIControl2 in array2)
		{
			gUIControl2.ShowImmediately();
		}
		GUIControl[] array3 = promoButtonsControls;
		foreach (GUIControl gUIControl3 in array3)
		{
			gUIControl3.HideImmediately();
		}
		SkeletonAnimation[] array4 = tapsAnimation;
		foreach (SkeletonAnimation skeletonAnimation in array4)
		{
			skeletonAnimation.gameObject.SetActive(false);
		}
	}

	private void OnEnable()
	{
		tk2dUIItem[] array = hitButtons;
		foreach (tk2dUIItem tk2dUIItem2 in array)
		{
			tk2dUIItem2.OnClick += ComputerButton_OnClick;
		}
		MenuScreen.OnChangeCurrentPhase += MenuScreen_OnChangeCurrentPhase;
	}

	private void OnDisable()
	{
		tk2dUIItem[] array = hitButtons;
		foreach (tk2dUIItem tk2dUIItem2 in array)
		{
			tk2dUIItem2.OnClick -= ComputerButton_OnClick;
		}
		MenuScreen.OnChangeCurrentPhase -= MenuScreen_OnChangeCurrentPhase;
	}

	public void WillChooseCharacter(CharacterNameType characterType)
	{
		if (currentPhase != MenuPhase.Mode && characterType != CharacterNameType.Raper)
		{
			if (CoroutineParty != null)
			{
				StopCoroutine(CoroutineParty);
				CoroutineParty = null;
			}
			if (CoroutineChangeSkin != null)
			{
				StopCoroutine(CoroutineChangeSkin);
				CoroutineChangeSkin = null;
			}
			PartyGameState = PartyGameStateType.None;
		}
	}

	public void DidChooseCharacter(CharacterNameType characterType)
	{
		if (characterType == CharacterNameType.Raper)
		{
			AvatarOnEnable();
			if (currentPhase != MenuPhase.Mode)
			{
				if (CoroutineParty != null)
				{
					StopCoroutine(CoroutineParty);
					CoroutineParty = null;
				}
				CoroutineParty = StartCoroutine(IEnumeratorParty());
			}
		}
		currentCharacter = characterType;
	}

	private void ComputerButton_OnClick()
	{
		if (PartyGameState == PartyGameStateType.Active)
		{
			girlsHealt -= 0.05f;
			tk2dProgressBar[] array = girlsProgressBar;
			foreach (tk2dProgressBar tk2dProgressBar2 in array)
			{
				tk2dProgressBar2.fillAmount = girlsHealt;
			}
			foreach (SkeletonAnimation avatar in avatars)
			{
				avatar.Skeleton.SetAttachment("raper_money", "raper_money");
				avatar.State.SetAnimation(1, RAPER_IDLE_HANDS_ANIMATION, true);
			}
			if (girlsHealt <= 0f)
			{
				PartyGameState = PartyGameStateType.Finish;
			}
		}
		tapCount++;
		if (tapCount >= 5)
		{
			IsTapAnimationActive = false;
		}
	}

	private void MenuScreen_OnChangeCurrentPhase(MenuPhase obj)
	{
		if (obj == MenuPhase.Mode || obj == MenuPhase.Tournament)
		{
			PartyGameState = PartyGameStateType.None;
		}
		else if (currentPhase == MenuPhase.Mode && currentCharacter == CharacterNameType.Raper)
		{
			GUIControl[] array = proposalsControls;
			foreach (GUIControl gUIControl in array)
			{
				gUIControl.ShowImmediately();
			}
			GUIControl[] array2 = flagsControls;
			foreach (GUIControl gUIControl2 in array2)
			{
				gUIControl2.ShowImmediately();
			}
			if (CoroutineParty != null)
			{
				StopCoroutine(CoroutineParty);
				CoroutineParty = null;
			}
			CoroutineParty = StartCoroutine(IEnumeratorParty());
		}
		currentPhase = obj;
	}

	private void Avatar_State_Event(Spine.AnimationState state, int trackIndex, Spine.Event e)
	{
		if (!(e.Data.Name == "Money_start_promo"))
		{
			return;
		}
		foreach (SkeletonAnimation avatar in avatars)
		{
			if (avatar.gameObject.activeInHierarchy && clothEffect != null)
			{
				SkeletonUtility component = avatar.GetComponent<SkeletonUtility>();
				SkeletonUtilityBone utilityBone = GetUtilityBone(component, "hand_ik_l");
				GameObject original = moneyEffect.GetAsset() as GameObject;
				GameObject gameObject = Object.Instantiate(original);
				ParticleSystem[] componentsInChildren = gameObject.GetComponentsInChildren<ParticleSystem>();
				foreach (ParticleSystem particleSystem in componentsInChildren)
				{
					particleSystem.Play();
				}
				gameObject.transform.parent = utilityBone.transform;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
				Object.Destroy(gameObject, 1f);
			}
		}
	}

	private SkeletonUtilityBone GetUtilityBone(SkeletonUtility characterUtility, string boneName)
	{
		SkeletonUtilityBone result = null;
		if (characterUtility != null && !string.IsNullOrEmpty(boneName))
		{
			for (int i = 0; i < characterUtility.utilityBones.Count; i++)
			{
				SkeletonUtilityBone skeletonUtilityBone = characterUtility.utilityBones[i];
				if (skeletonUtilityBone != null && skeletonUtilityBone.boneName == boneName)
				{
					result = skeletonUtilityBone;
					break;
				}
			}
		}
		return result;
	}

	private void AvatarOnEnable()
	{
		AvatarOnDisable();
		Dictionary<GUILayoutCell, SkeletonAnimation> dictionary = null;
		dictionary = base.CachedManagers.CachedGUIScreenManager.MenuScreen.MenuScreenScrollView.AnimationAvatar;
		List<GUILayoutCell> list = new List<GUILayoutCell>(dictionary.Keys);
		foreach (GUILayoutCell item in list)
		{
			SkeletonAnimation skeletonAnimation = dictionary[item];
			skeletonAnimation.State.Event += Avatar_State_Event;
			avatars.Add(skeletonAnimation);
		}
	}

	private void AvatarOnDisable()
	{
		foreach (SkeletonAnimation avatar in avatars)
		{
			avatar.State.Event -= Avatar_State_Event;
		}
		avatars.Clear();
	}

	private void skinClear()
	{
		foreach (SkeletonAnimation avatar in avatars)
		{
			avatar.Skeleton.SetAttachment("snapback", "snapback");
			avatar.Skeleton.SetAttachment("eyes", "glasses");
			avatar.Skeleton.SetAttachment("head", "head");
			avatar.Skeleton.SetAttachment("body", "body");
			avatar.Skeleton.SetAttachment("arm_r", "hand1_1");
			avatar.Skeleton.SetAttachment("arm_l", "hand2_1");
			avatar.Skeleton.SetAttachment("forearm_r", "hand1_2");
			avatar.Skeleton.SetAttachment("forearm_l", "hand2_2");
			avatar.Skeleton.SetAttachment("hand_r", "hand1_3");
			avatar.Skeleton.SetAttachment("hand_l", "hand2_3");
			avatar.Skeleton.SetAttachment("thigh_r", "leg1_1");
			avatar.Skeleton.SetAttachment("thigh_l", "leg2_1");
			avatar.Skeleton.SetAttachment("shin_r", "leg1_2");
			avatar.Skeleton.SetAttachment("shin_l", "leg2_2");
			avatar.Skeleton.SetAttachment("foot_r", "leg1_3");
			avatar.Skeleton.SetAttachment("foot_l", "leg2_3");
		}
	}

	private IEnumerator IEnumeratorParty()
	{
		yield return new WaitForSeconds(1f);
		PartyGameState = PartyGameStateType.Active;
		if (CoroutineChangeSkin != null)
		{
			StopCoroutine(CoroutineChangeSkin);
			CoroutineChangeSkin = null;
		}
		CoroutineChangeSkin = StartCoroutine(IEnumeratorChangeSkin());
	}

	private IEnumerator IEnumeratorChangeSkin()
	{
		foreach (SkeletonAnimation avatar in avatars)
		{
			avatar.State.SetAnimation(0, RAPER_IDLE_ANIMATION, true);
			avatar.State.SetAnimation(1, RAPER_IDLE_HANDS_ANIMATION, true);
		}
		foreach (SkeletonAnimation avatar2 in avatars)
		{
			if (avatar2.gameObject.activeInHierarchy && clothEffect != null)
			{
				SkeletonUtility component = avatar2.GetComponent<SkeletonUtility>();
				SkeletonUtilityBone utilityBone = GetUtilityBone(component, "hat");
				GameObject original = clothEffect.GetAsset() as GameObject;
				GameObject gameObject = Object.Instantiate(original);
				ParticleSystem[] componentsInChildren = gameObject.GetComponentsInChildren<ParticleSystem>();
				foreach (ParticleSystem particleSystem in componentsInChildren)
				{
					particleSystem.Play();
				}
				gameObject.transform.parent = utilityBone.transform;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
			}
		}
		yield return new WaitForSeconds(0.1f);
		foreach (SkeletonAnimation avatar3 in avatars)
		{
			avatar3.Skeleton.SetAttachment("snapback", "hat_promo");
			avatar3.Skeleton.SetAttachment("raper_money", "raper_money");
		}
		foreach (SkeletonAnimation avatar4 in avatars)
		{
			if (avatar4.gameObject.activeInHierarchy && clothEffect != null)
			{
				SkeletonUtility component2 = avatar4.GetComponent<SkeletonUtility>();
				SkeletonUtilityBone utilityBone2 = GetUtilityBone(component2, "eyes");
				GameObject original2 = clothEffect.GetAsset() as GameObject;
				GameObject gameObject2 = Object.Instantiate(original2);
				ParticleSystem[] componentsInChildren2 = gameObject2.GetComponentsInChildren<ParticleSystem>();
				foreach (ParticleSystem particleSystem2 in componentsInChildren2)
				{
					particleSystem2.Play();
				}
				gameObject2.transform.parent = utilityBone2.transform;
				gameObject2.transform.localPosition = Vector3.zero;
				gameObject2.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
			}
		}
		yield return new WaitForSeconds(0.1f);
		foreach (SkeletonAnimation avatar5 in avatars)
		{
			avatar5.Skeleton.SetAttachment("eyes", "glasses_promo");
			avatar5.Skeleton.SetAttachment("head", "head_promo");
		}
		foreach (SkeletonAnimation avatar6 in avatars)
		{
			if (avatar6.gameObject.activeInHierarchy && clothEffect != null)
			{
				SkeletonUtility component3 = avatar6.GetComponent<SkeletonUtility>();
				SkeletonUtilityBone utilityBone3 = GetUtilityBone(component3, "chain_link_1");
				GameObject original3 = clothEffect.GetAsset() as GameObject;
				GameObject gameObject3 = Object.Instantiate(original3);
				ParticleSystem[] componentsInChildren3 = gameObject3.GetComponentsInChildren<ParticleSystem>();
				foreach (ParticleSystem particleSystem3 in componentsInChildren3)
				{
					particleSystem3.Play();
				}
				gameObject3.transform.parent = utilityBone3.transform;
				gameObject3.transform.localPosition = Vector3.zero;
			}
		}
		yield return new WaitForSeconds(0.1f);
		foreach (SkeletonAnimation avatar7 in avatars)
		{
			avatar7.Skeleton.SetAttachment("body", "body_promo");
			avatar7.Skeleton.SetAttachment("arm_r", "shoulder_r_promo");
			avatar7.Skeleton.SetAttachment("arm_l", "shoulder_l_promo");
			avatar7.Skeleton.SetAttachment("forearm_r", "elbow_r_promo");
			avatar7.Skeleton.SetAttachment("forearm_l", "elbow_l_promo");
			avatar7.Skeleton.SetAttachment("hand_r", "hand_r_promo");
			avatar7.Skeleton.SetAttachment("hand_l", "hand_l_promo");
		}
		foreach (SkeletonAnimation avatar8 in avatars)
		{
			if (avatar8.gameObject.activeInHierarchy && clothEffect != null)
			{
				SkeletonUtility component4 = avatar8.GetComponent<SkeletonUtility>();
				SkeletonUtilityBone utilityBone4 = GetUtilityBone(component4, "shin_l");
				GameObject original4 = clothEffect.GetAsset() as GameObject;
				GameObject gameObject4 = Object.Instantiate(original4);
				ParticleSystem[] componentsInChildren4 = gameObject4.GetComponentsInChildren<ParticleSystem>();
				foreach (ParticleSystem particleSystem4 in componentsInChildren4)
				{
					particleSystem4.Play();
				}
				gameObject4.transform.parent = utilityBone4.transform;
				gameObject4.transform.localPosition = Vector3.zero;
			}
		}
		yield return new WaitForSeconds(0.1f);
		foreach (SkeletonAnimation avatar9 in avatars)
		{
			avatar9.Skeleton.SetAttachment("thigh_r", "thigh_r_promo");
			avatar9.Skeleton.SetAttachment("thigh_l", "thigh_l_promo");
			avatar9.Skeleton.SetAttachment("shin_r", "shin_r_promo");
			avatar9.Skeleton.SetAttachment("shin_l", "shin_l_promo");
			avatar9.Skeleton.SetAttachment("foot_r", "foot_r_promo");
			avatar9.Skeleton.SetAttachment("foot_l", "foot_l_promo");
		}
		IsTapAnimationActive = true;
	}
}
