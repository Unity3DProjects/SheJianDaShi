using System;
using UnityEngine;

public class DubStepGrenade : WeaponGeneral
{
	private int EXPLOSION_SOUNDS_COUNT = 8;

	[SerializeField]
	private int reboundQuantity;

	[SerializeField]
	private float minReflectStrength = 500f;

	[SerializeField]
	private float maxReflectStrength = 1000f;

	private int currentReboundQuantity;

	private float delayBetweenCollisions;

	private SoundItem explosionSound;

	private int explosionsCounter;

	private int[] randomPermutation;

	private int ExplosionSoundIndex
	{
		get
		{
			if (randomPermutation == null || explosionsCounter >= EXPLOSION_SOUNDS_COUNT)
			{
				randomPermutation = new int[EXPLOSION_SOUNDS_COUNT];
				for (int i = 0; i < EXPLOSION_SOUNDS_COUNT; i++)
				{
					randomPermutation[i] = i;
				}
				for (int j = 0; j < EXPLOSION_SOUNDS_COUNT - 1; j++)
				{
					int num = UnityEngine.Random.Range(j, EXPLOSION_SOUNDS_COUNT);
					int num2 = randomPermutation[j];
					randomPermutation[j] = randomPermutation[num];
					randomPermutation[num] = num2;
				}
				explosionsCounter = 0;
			}
			else
			{
				explosionsCounter++;
			}
			return randomPermutation[explosionsCounter];
		}
	}

	private SoundItem ExplosionSound
	{
		get
		{
			if (explosionSound == null)
			{
				SoundsCategory[] audioCategories = SingletonMonoBehaviour<SoundsManager>.Instance.AudioCategories;
				foreach (SoundsCategory soundsCategory in audioCategories)
				{
					for (int j = 0; j < soundsCategory.AudioItems.Length; j++)
					{
						if (soundsCategory.AudioItems[j].Name == "dubstep_grenade_explosion")
						{
							explosionSound = soundsCategory.AudioItems[j];
							return explosionSound;
						}
					}
				}
			}
			return explosionSound;
		}
	}

	protected override void OnCollisionEnter(Collision collision)
	{
		base.OnCollisionEnter(collision);
		if (base.IsActive)
		{
			float num = 0f;
			float num2 = 0f;
			if (SingletonMonoBehaviour<GameManager>.Instance.CurrentGameType == GameType.Multiplayer)
			{
				num = Mathf.Atan2(base.Velocity.y, base.Velocity.x) - (float)Math.PI + 0.02f;
				num2 = (minReflectStrength + maxReflectStrength) * 0.5f;
			}
			else
			{
				num = Mathf.Atan2(base.Velocity.y, base.Velocity.x) - (float)Math.PI + UnityEngine.Random.Range(-(float)Math.PI / 10f, (float)Math.PI / 10f);
				num2 = UnityEngine.Random.Range(minReflectStrength, maxReflectStrength);
			}
			base.CachedRigidbody.velocity = (velocity = new Vector3(Mathf.Cos(num) * num2, Mathf.Sin(num) * num2));
		}
	}

	public override void Activate(WeaponActivationType type, ICollidable collidedObject)
	{
		currentReboundQuantity++;
		base.Activate(type, collidedObject);
	}

	public override void UpdateWeapon(float deltaTime)
	{
		base.UpdateWeapon(deltaTime);
		delayBetweenCollisions += deltaTime;
		if (delayBetweenCollisions >= 0.1f && !isCollideEnabled)
		{
			delayBetweenCollisions = 0f;
			isCollideEnabled = true;
		}
	}

	protected override void OnHitActivation()
	{
		bool flag = currentReboundQuantity > reboundQuantity;
		ExplodeComponent component = GetComponent<ExplodeComponent>();
		if (component != null)
		{
			component.Explode(this, level, flag, flag);
			PlayExplosionSoundWithoutRepeat();
		}
	}

	protected void PlayExplosionSoundWithoutRepeat()
	{
		AudioListener currentAudioListener = SoundsManager.GetCurrentAudioListener();
		SoundSubItem subItem = ExplosionSound.subItems[ExplosionSoundIndex];
		SingletonMonoBehaviour<SoundsManager>.Instance.PlayAudioSubItem(explosionSound.Category, explosionSound, subItem, currentAudioListener.transform.position + currentAudioListener.transform.forward, null, false, null);
	}
}
