using UnityEngine;

public class Jackhammer : WeaponGeneral
{
	private const string DEFAULT_ANIMATION = "default";

	private const string IDLE_ANIMATION = "idle";

	[SerializeField]
	private float timeFountain;

	[SerializeField]
	private float forceFountain;

	[SerializeField]
	private float damageFountain;

	[SerializeField]
	private float radiusFountain;

	[SerializeField]
	private float activationTimer;

	[SerializeField]
	private int joltFrequency;

	[SerializeField]
	private int joltCount;

	[SerializeField]
	private float joltForce;

	[SerializeField]
	private SkeletonAnimation skeletonAnimation;

	private bool isActivate;

	private CharacterBase currentCharacter;

	private int currentJoltNumber;

	private bool isJolting;

	private float timer;

	public override void UpdateWeapon(float deltaTime)
	{
		base.UpdateWeapon(deltaTime);
		if (!isJolting)
		{
			return;
		}
		timer += deltaTime;
		if (!(timer > activationTimer))
		{
			return;
		}
		if ((int)((timer - activationTimer) * (float)joltFrequency) != currentJoltNumber)
		{
			currentJoltNumber = (int)((timer - activationTimer) * (float)joltFrequency);
			if (currentJoltNumber < joltCount)
			{
				currentJoltNumber = (int)((timer - activationTimer) * (float)joltFrequency);
				if (currentCharacter != null)
				{
					currentCharacter.RootRigidBody.AddRelativeForce(0f, joltForce * ((float)(currentJoltNumber % 2) - 0.5f) * 1000000f, 0f);
				}
			}
			else
			{
				isJolting = false;
				currentJoltNumber = 0;
				timer = 0f;
				ApplyExplosion(currentCharacter.BodyTransform.position);
				level.TurnActionsChangeCount(-1);
				skeletonAnimation.State.SetAnimation(0, "default", true);
				base.IdleEffect.StopFX(false);
			}
		}
		if (base.CachedRigidbody != null)
		{
			Object.Destroy(base.CachedRigidbody);
		}
	}

	public override ICollidable Collided(CollisionInfo collisionInfo)
	{
		level.TurnActionsChangeCount(1);
		collisionInfo.isDisableRestore = true;
		collisionInfo.isDisableFatality = true;
		ICollidable collidable = base.Collided(collisionInfo);
		if (collidable is CharacterBase)
		{
			currentCharacter = collidable as CharacterBase;
			skeletonAnimation.MeshRenderer.sortingLayerName = "UnderCharacter";
			if (!currentCharacter.IsFinishHim)
			{
				timer = 0f;
				isJolting = true;
			}
			else
			{
				ApplyExplosion(currentCharacter.BodyTransform.position);
				skeletonAnimation.State.SetAnimation(0, "default", true);
				base.IdleEffect.StopFX(true);
				characterPierceSound = string.Empty;
				Object.Destroy(this);
			}
		}
		else
		{
			level.TurnActionsChangeCount(-1);
			skeletonAnimation.State.SetAnimation(0, "default", true);
			base.IdleEffect.StopFX(true);
		}
		return collidable;
	}

	private void ApplyExplosion(Vector3 position)
	{
		ExplosionInfo explosionInfo = new ExplosionInfo
		{
			weaponType = base.WeaponType,
			damage = damageFountain,
			force = forceFountain * 3000000f,
			position = new Vector3(position.x, position.y - 100f, position.z),
			radius = radiusFountain
		};
		currentCharacter.ApplyExplosionForce(explosionInfo);
	}
}
