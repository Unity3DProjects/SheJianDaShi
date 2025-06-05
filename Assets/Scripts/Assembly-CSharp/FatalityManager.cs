using System;
using System.Collections.Generic;
using Spine;
using UnityEngine;

public class FatalityManager : MonoBehaviour
{
	private const string DEATH_ACID = "death_by_fire";

	private const string DEATH_SHOCK = "death_by_shock";

	private const string DEATH_FIRE = "death_by_fire";

	private const string DEATH_MAGIC_SKULL = "death_by_skull";

	private const string DEATH_JACKHAMMER = "death_by_bur";

	private const string DEATH_END = "Death_End";

	private const string DEATH_GRENADE = "death_by_grenade";

	private const string DEATH_FREEZE = "death_by_freeze";

	private const string DEATH_GUILLOTINE = "death_by_guillotine";

	private const string DEATH_GUILLOTINE_POSE = "death_by_guillotine_pose";

	private const string DEATH_ADAM_X_FATALITY = "adam_x_fatality";

	private const string DEATH_HOLE = "death_by_hole";

	private const string DEATH_PLASMA = "death_by_plasma";

	private const string DEATH_BY_HEADCRAB = "death_by_headcrab";

	private const string ANIMATION_FACE = "face_ko";

	public Action<WeaponType> OnFatalityEnd;

	[SerializeField]
	private Fatalities fatalities;

	[SerializeField]
	private CharacterBase character;

	[SerializeField]
	private SkeletonRagdoll characterRagdoll;

	[SerializeField]
	private SkeletonAnimation characterAnimation;

	[SerializeField]
	private MaskManager characterMaskManager;

	[SpineBone("", "characterAnimation")]
	[SerializeField]
	private string boneHead;

	[SpineBone("", "characterAnimation")]
	[SerializeField]
	private string boneBody;

	[SpineBone("", "characterAnimation")]
	[SerializeField]
	private string[] bonesLeg;

	[SpineBone("", "characterAnimation")]
	[SerializeField]
	private string[] bonesArm;

	private Dictionary<Collider, SkeletonUtilityBone> cachedColliderBones = new Dictionary<Collider, SkeletonUtilityBone>();

	private List<SkeletonUtilityBone> cachedUtilityBones = new List<SkeletonUtilityBone>();

	private Dictionary<WeaponType, List<FatalityBase>> currentFatality = new Dictionary<WeaponType, List<FatalityBase>>();

	private FatalityBase activeFatality;

	private Spine.Animation animationFace;

	private Spine.Animation animationAcid;

	private Spine.Animation animationFire;

	private Spine.Animation animationMagicSkull;

	private Spine.Animation animationJackhammer;

	private Spine.Animation animationShock;

	private Spine.Animation animationGrenade;

	private Spine.Animation animationFreeze;

	private Spine.Animation animationGuillotine;

	private Spine.Animation animationGuillotinePose;

	private Spine.Animation animationAdamXFatality;

	private Spine.Animation animationPlasma;

	private Spine.Animation animationHole;

	private Spine.Animation animationHeadcrabDeath;

	private EventData animationDeathEnd;

	private Level level;

	public FatalityBase ActiveFatality
	{
		get
		{
			return activeFatality;
		}
	}

	public CharacterBase CurrentCharacter
	{
		get
		{
			return character;
		}
	}

	public SkeletonAnimation CharacterAnimation
	{
		get
		{
			return characterAnimation;
		}
	}

	public SkeletonRagdoll CharacterRagdoll
	{
		get
		{
			return characterRagdoll;
		}
	}

	public MaskManager CharacterMaskManager
	{
		get
		{
			return characterMaskManager;
		}
	}

	public Spine.Animation AnimationFace
	{
		get
		{
			if (animationFace == null)
			{
				animationFace = characterAnimation.state.Data.SkeletonData.FindAnimation("face_ko");
			}
			return animationFace;
		}
	}

	public Spine.Animation AnimationAcid
	{
		get
		{
			if (animationAcid == null)
			{
				animationAcid = characterAnimation.Skeleton.Data.FindAnimation("death_by_fire");
			}
			return animationAcid;
		}
	}

	public Spine.Animation AnimationFire
	{
		get
		{
			if (animationFire == null)
			{
				animationFire = characterAnimation.Skeleton.Data.FindAnimation("death_by_fire");
			}
			return animationFire;
		}
	}

	public Spine.Animation AnimationMagicSkull
	{
		get
		{
			if (animationMagicSkull == null)
			{
				animationMagicSkull = characterAnimation.Skeleton.Data.FindAnimation("death_by_skull");
			}
			return animationMagicSkull;
		}
	}

	public Spine.Animation AnimationJackhammer
	{
		get
		{
			if (animationJackhammer == null)
			{
				animationJackhammer = characterAnimation.Skeleton.Data.FindAnimation("death_by_bur");
			}
			return animationJackhammer;
		}
	}

	public Spine.Animation AnimationShock
	{
		get
		{
			if (animationShock == null)
			{
				animationShock = characterAnimation.Skeleton.Data.FindAnimation("death_by_shock");
			}
			return animationShock;
		}
	}

	public Spine.Animation AnimationGrenade
	{
		get
		{
			if (animationGrenade == null)
			{
				animationGrenade = characterAnimation.Skeleton.Data.FindAnimation("death_by_grenade");
			}
			return animationGrenade;
		}
	}

	public Spine.Animation AnimationFreeze
	{
		get
		{
			if (animationFreeze == null)
			{
				animationFreeze = characterAnimation.Skeleton.Data.FindAnimation("death_by_freeze");
			}
			return animationFreeze;
		}
	}

	public Spine.Animation AnimationGuillotine
	{
		get
		{
			if (animationGuillotine == null)
			{
				animationGuillotine = characterAnimation.Skeleton.Data.FindAnimation("death_by_guillotine");
			}
			return animationGuillotine;
		}
	}

	public Spine.Animation AnimationGuillotinePose
	{
		get
		{
			if (animationGuillotinePose == null)
			{
				animationGuillotinePose = characterAnimation.Skeleton.Data.FindAnimation("death_by_guillotine_pose");
			}
			return animationGuillotinePose;
		}
	}

	public Spine.Animation AnimationAdamXFatality
	{
		get
		{
			if (animationAdamXFatality == null)
			{
				animationAdamXFatality = characterAnimation.Skeleton.Data.FindAnimation("adam_x_fatality");
			}
			return animationAdamXFatality;
		}
	}

	public Spine.Animation AnimationHole
	{
		get
		{
			if (animationHole == null)
			{
				animationHole = characterAnimation.Skeleton.Data.FindAnimation("death_by_hole");
			}
			return animationHole;
		}
	}

	public Spine.Animation AnimationPlasma
	{
		get
		{
			if (animationPlasma == null)
			{
				animationPlasma = characterAnimation.Skeleton.Data.FindAnimation("death_by_plasma");
			}
			return animationPlasma;
		}
	}

	public Spine.Animation AnimationHeadcrabDeath
	{
		get
		{
			if (animationHeadcrabDeath == null)
			{
				animationHeadcrabDeath = characterAnimation.Skeleton.Data.FindAnimation("death_by_headcrab");
			}
			return animationHeadcrabDeath;
		}
	}

	public EventData AnimationDeathEnd
	{
		get
		{
			if (animationDeathEnd == null)
			{
				animationDeathEnd = characterAnimation.Skeleton.Data.FindEvent("Death_End");
			}
			return animationDeathEnd;
		}
	}

	public List<SkeletonUtilityBone> CachedUtilityBones
	{
		get
		{
			return cachedUtilityBones;
		}
	}

	private void Start()
	{
		Collider[] componentsInChildren = characterAnimation.GetComponentsInChildren<Collider>();
		if (componentsInChildren != null)
		{
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				SkeletonUtilityBone component = componentsInChildren[i].transform.parent.GetComponent<SkeletonUtilityBone>();
				if (component != null)
				{
					cachedColliderBones.Add(componentsInChildren[i], component);
				}
			}
		}
		characterAnimation.GetComponentsInChildren(cachedUtilityBones);
		if (!(fatalities != null))
		{
			return;
		}
		for (int j = 0; j < fatalities.FatalityPrefabs.Count; j++)
		{
			if (fatalities.FatalityPrefabs[j] != null)
			{
				FatalityBase fatalityBase = UnityEngine.Object.Instantiate(fatalities.FatalityPrefabs[j]);
				fatalityBase.transform.parent = base.transform;
				fatalityBase.transform.localEulerAngles = Vector3.zero;
				fatalityBase.transform.localPosition = Vector3.zero;
				if (!currentFatality.ContainsKey(fatalities.FatalityPrefabs[j].TypeWeapon))
				{
					currentFatality.Add(fatalities.FatalityPrefabs[j].TypeWeapon, new List<FatalityBase>());
				}
				currentFatality[fatalities.FatalityPrefabs[j].TypeWeapon].Add(fatalityBase);
				fatalityBase.InitFatality(this);
			}
		}
	}

	public void Init(Level parenLevel)
	{
		level = parenLevel;
	}

	public void StartFatality(CollisionInfo infoCollision)
	{
		if (activeFatality == null)
		{
			level.UnfollowWeaponWithVelocity(Vector2.zero);
			if (currentFatality.ContainsKey(infoCollision.fatality))
			{
				List<FatalityBase> list = currentFatality[infoCollision.fatality];
				FatalityBase fatalityBase = list.Find((FatalityBase obj) => (obj.ActivationType & infoCollision.fatalityActvationType) != 0);
				if ((bool)fatalityBase)
				{
					level.TurnActionsChangeCount(1);
					activeFatality = fatalityBase;
					activeFatality.StartFatality(infoCollision);
					FatalityBase fatalityBase2 = activeFatality;
					fatalityBase2.OnFatalityFinish = (Action<FatalityBase>)Delegate.Combine(fatalityBase2.OnFatalityFinish, new Action<FatalityBase>(FatalityBase_OnFatalityFinish));
				}
			}
			else if (OnFatalityEnd != null)
			{
				OnFatalityEnd(infoCollision.fatality);
			}
		}
		else if (activeFatality.TypeWeapon == infoCollision.fatality)
		{
			activeFatality.StartFatality(infoCollision);
		}
	}

	public void UpdateFatality(float deltaTime)
	{
		if (activeFatality != null)
		{
			activeFatality.UpdateFatality(deltaTime);
		}
	}

	public void StopFatality()
	{
		if (activeFatality != null)
		{
			activeFatality.StopFatality();
			FatalityBase fatalityBase = activeFatality;
			fatalityBase.OnFatalityFinish = (Action<FatalityBase>)Delegate.Remove(fatalityBase.OnFatalityFinish, new Action<FatalityBase>(FatalityBase_OnFatalityFinish));
			activeFatality = null;
			level.TurnActionsChangeCount(-1);
		}
	}

	public SkeletonUtilityBone GetUtilityBone(Collider collider)
	{
		SkeletonUtilityBone result = null;
		if (cachedColliderBones.ContainsKey(collider))
		{
			result = cachedColliderBones[collider];
		}
		return result;
	}

	public SkeletonUtilityBone GetUtilityBone(string nameBone)
	{
		SkeletonUtilityBone result = null;
		if (!string.IsNullOrEmpty(nameBone) && cachedUtilityBones != null)
		{
			for (int i = 0; i < cachedUtilityBones.Count; i++)
			{
				if (cachedUtilityBones[i].boneName == nameBone)
				{
					result = cachedUtilityBones[i];
					break;
				}
			}
		}
		return result;
	}

	public SkeletonUtilityBone GetUtilityBoneByPosition(Vector3 position)
	{
		SkeletonUtilityBone result = null;
		List<SkeletonUtilityBone> list = new List<SkeletonUtilityBone>(cachedColliderBones.Values);
		float num = float.MaxValue;
		for (int i = 0; i < list.Count; i++)
		{
			if ((list[i].CachedTransform.position - position).magnitude < num)
			{
				num = (list[i].CachedTransform.position - position).magnitude;
				result = list[i];
			}
		}
		return result;
	}

	public Collider GetCollider(SkeletonUtilityBone utilityBone)
	{
		Collider result = null;
		if (cachedColliderBones.ContainsValue(utilityBone))
		{
			foreach (Collider key in cachedColliderBones.Keys)
			{
				if (utilityBone == cachedColliderBones[key])
				{
					result = key;
					break;
				}
			}
		}
		return result;
	}

	public SkeletonUtilityBone[] GetUtilityBone(string[] nameBones)
	{
		SkeletonUtilityBone[] array = null;
		if (nameBones != null)
		{
			array = new SkeletonUtilityBone[nameBones.Length];
			for (int i = 0; i < nameBones.Length; i++)
			{
				if (!string.IsNullOrEmpty(nameBones[i]))
				{
					array[i] = GetUtilityBone(nameBones[i]);
				}
			}
		}
		return array;
	}

	public SkeletonUtilityBone GetUtilityBoneHead()
	{
		return GetUtilityBone(boneHead);
	}

	public SkeletonUtilityBone GetUtilityBoneBody()
	{
		return GetUtilityBone(boneBody);
	}

	public SkeletonUtilityBone[] GetUtilityBonesArms()
	{
		return GetUtilityBone(bonesArm);
	}

	public SkeletonUtilityBone[] GetUtilityBonesLegs()
	{
		return GetUtilityBone(bonesLeg);
	}

	public bool WithCharacterCollision(Collider collider)
	{
		bool result = false;
		if (cachedColliderBones != null)
		{
			result = cachedColliderBones.ContainsKey(collider);
		}
		return result;
	}

	private void FatalityBase_OnFatalityFinish(FatalityBase sender)
	{
		if (activeFatality == sender)
		{
			level.TurnActionsChangeCount(1);
			StopFatality();
			ShopItemCharacter shopItemCharacter = SingletonMonoBehaviour<LevelsManager>.Instance.ObjectsSpawner.GetShopItemCharacter(level.ActiveCharacter.CharacterNameType);
			if (shopItemCharacter.UpgradeLevel == 0)
			{
				SingletonMonoBehaviour<MicroachievementsManager>.Instance.CheckLevelAchievementReaching(MicroachievementType.Fatality);
				AudioManager.PlayAudioWithKey("fatality");
			}
			else
			{
				SingletonMonoBehaviour<MicroachievementsManager>.Instance.CheckLevelAchievementReaching(MicroachievementType.Brutality);
			}
			if (OnFatalityEnd != null)
			{
				OnFatalityEnd(sender.TypeWeapon);
			}
			else
			{
				level.TurnActionsChangeCount(-1);
			}
		}
	}
}
