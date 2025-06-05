using System;
using System.Collections.Generic;
using Spine;
using UnityEngine;

public class FatalityPulse : FatalityBase
{
	private class BoneInfo
	{
		public float updateTime;

		public float currentTime;

		public SkeletonUtilityBone currentBone;

		public Action<BoneInfo, float> currentUpdate;

		public Slot[] currentSlots;

		public BoneInfo(SkeletonUtilityBone bone, float time, Action<BoneInfo, float> update)
		{
			updateTime = time;
			currentUpdate = update;
			currentBone = bone;
			currentTime = 0f;
			currentSlots = null;
		}
	}

	[SerializeField]
	private AnimationCurve pulseCurve;

	[SerializeField]
	private float timePulse = 1f;

	[SerializeField]
	private float randomDelay;

	[SerializeField]
	private FatalityParts pulseParts;

	private List<BoneInfo> currentPartsUpdate = new List<BoneInfo>();

	private GameObject currentBrainEffect;

	private GameObject currentGutsEffect;

	private SkeletonUtilityBone cachedHead;

	private SkeletonUtilityBone cachedBody;

	private List<Slot> tempSlots = new List<Slot>();

	private bool shouldCrowdHappySoundPlay;

	protected SkeletonUtilityBone CachedHead
	{
		get
		{
			if (cachedHead == null)
			{
				cachedHead = base.CachedManager.GetUtilityBoneHead();
			}
			return cachedHead;
		}
	}

	protected SkeletonUtilityBone CachedBody
	{
		get
		{
			if (cachedBody == null)
			{
				cachedBody = base.CachedManager.GetUtilityBoneBody();
			}
			return cachedBody;
		}
	}

	protected bool ShouldCrowdHappySoundPlay
	{
		get
		{
			return shouldCrowdHappySoundPlay;
		}
		set
		{
			if (shouldCrowdHappySoundPlay != value)
			{
				shouldCrowdHappySoundPlay = value;
				if (shouldCrowdHappySoundPlay)
				{
					base.PlayCrowdHappySound();
				}
			}
		}
	}

	public override void StartFatality(CollisionInfo infoCollision)
	{
		if (!base.IsActive)
		{
			base.StartFatality(infoCollision);
			base.CachedManager.CurrentCharacter.ApplyCharacterRagdoll();
			if ((FatalityParts.Head & pulseParts) != FatalityParts.None)
			{
				AddBone(base.CachedManager.GetUtilityBoneHead());
			}
			if ((FatalityParts.Arms & pulseParts) != FatalityParts.None)
			{
				AddBones(base.CachedManager.GetUtilityBonesArms());
			}
			if ((FatalityParts.Legs & pulseParts) != FatalityParts.None)
			{
				AddBones(base.CachedManager.GetUtilityBonesLegs());
			}
			if ((FatalityParts.Body & pulseParts) != FatalityParts.None)
			{
				AddBone(base.CachedManager.GetUtilityBoneBody());
			}
		}
	}

	public override void UpdateFatality(float deltaTime)
	{
		base.UpdateFatality(deltaTime);
		if (!base.IsActive || currentPartsUpdate == null)
		{
			return;
		}
		int i = 0;
		for (int count = currentPartsUpdate.Count; i < count; i++)
		{
			BoneInfo boneInfo = currentPartsUpdate[i];
			if (boneInfo.currentUpdate != null)
			{
				boneInfo.currentUpdate(boneInfo, deltaTime);
			}
		}
	}

	public override void StopFatality()
	{
		base.StopFatality();
		currentPartsUpdate.Clear();
	}

	protected virtual void ExplodePart(SkeletonUtilityBone boneUtility)
	{
		base.CachedEffect.EmitExplodeEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, boneUtility.transform.position, boneUtility.transform.eulerAngles);
		if (boneUtility == CachedHead)
		{
			CreateBloodEffects();
			base.CachedEffect.EmitBrainEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, CachedBody.transform.position, CachedBody.transform.eulerAngles);
			base.CachedEffect.EmitGutsEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, CachedHead.transform.position, CachedHead.transform.eulerAngles);
			base.CachedEffect.EmitBrain(base.transform, boneUtility.transform.position, Vector3.up);
			base.CachedEffect.EmitGuts(base.transform, CachedHead.transform.position, Vector3.down);
			base.CachedMask.Activate(startFatalityCollider, startFatalityUtilityBone);
			base.CachedManager.CurrentCharacter.ApplyCharacterRagdoll();
		}
		ShouldCrowdHappySoundPlay = true;
	}

	protected virtual void CreateBloodEffects()
	{
		base.CachedEffect.EmitBloodEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, CachedBody.transform.position, CachedBody.transform.eulerAngles);
	}

	protected override void PlayFatalityStartSound()
	{
		base.PlayFatalityStartSound();
		AudioManager.PlayAudioWithKey("fatality_pulse");
	}

	protected override void PlayCrowdHappySound()
	{
	}

	private void AddBone(SkeletonUtilityBone utilityBone)
	{
		if (!(utilityBone != null))
		{
			return;
		}
		float num = UnityEngine.Random.Range(0f, randomDelay);
		BoneInfo boneInfo = new BoneInfo(utilityBone, num, UpdateDelay);
		tempSlots.Clear();
		for (int i = 0; i < base.CachedManager.CharacterAnimation.Skeleton.Slots.Count; i++)
		{
			Slot slot = base.CachedManager.CharacterAnimation.Skeleton.Slots.Items[i];
			if (slot != null && slot.Bone == utilityBone.bone)
			{
				tempSlots.Add(slot);
			}
		}
		boneInfo.currentSlots = tempSlots.ToArray();
		currentPartsUpdate.Add(boneInfo);
	}

	private void AddBones(SkeletonUtilityBone[] utilityBones)
	{
		if (utilityBones != null)
		{
			for (int i = 0; i < utilityBones.Length; i++)
			{
				AddBone(utilityBones[i]);
			}
		}
	}

	private void UpdateDelay(BoneInfo info, float deltaTime)
	{
		info.currentTime += deltaTime;
		if (info.currentTime > info.updateTime)
		{
			info.currentTime -= info.updateTime;
			info.updateTime = timePulse;
			info.currentUpdate = UpdatePulse;
			info.currentUpdate(info, 0f);
		}
	}

	private void UpdatePulse(BoneInfo info, float deltaTime)
	{
		info.currentTime += deltaTime;
		if (info.currentTime < info.updateTime)
		{
			Slot[] currentSlots = info.currentSlots;
			if (currentSlots == null)
			{
				return;
			}
			foreach (Slot slot in currentSlots)
			{
				if (slot != null)
				{
					RegionAttachment regionAttachment = slot.Attachment as RegionAttachment;
					if (regionAttachment != null)
					{
						regionAttachment.ScaleX = pulseCurve.Evaluate(info.currentTime / timePulse);
						regionAttachment.ScaleY = pulseCurve.Evaluate(info.currentTime / timePulse);
						regionAttachment.UpdateOffset();
					}
				}
			}
			return;
		}
		Slot[] currentSlots2 = info.currentSlots;
		if (currentSlots2 != null)
		{
			foreach (Slot slot2 in currentSlots2)
			{
				if (slot2 != null)
				{
					RegionAttachment regionAttachment2 = slot2.Attachment as RegionAttachment;
					if (regionAttachment2 != null)
					{
						regionAttachment2.ScaleX = 1f;
						regionAttachment2.ScaleY = 1f;
						regionAttachment2.UpdateOffset();
					}
				}
			}
		}
		info.currentTime -= info.updateTime;
		info.currentUpdate = null;
		ExplodePart(info.currentBone);
	}
}
