using System.Collections.Generic;
using UnityEngine;

public class Apple : LevelObject, ICollidable
{
	[SerializeField]
	[ResourceLink]
	private AssetLink appleDestroyEffectPrefab;

	[SerializeField]
	private float forceCoeficent;

	[SerializeField]
	private float forceDivider;

	[SerializeField]
	private int coinsMin;

	[SerializeField]
	private int coinsMax;

	[SerializeField]
	private Collider appleDestroyCollider;

	private bool onHead;

	private LevelAppleShoot level;

	private bool isDestroyed;

	private Managers cachedManagers;

	private List<Collider> affectedObjects = new List<Collider>();

	public bool OnHead
	{
		get
		{
			return onHead && GetComponent<HingeJoint>() != null;
		}
		set
		{
			onHead = value;
		}
	}

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

	public void Init(LevelAppleShoot level)
	{
		this.level = level;
	}

	private void OnTriggerEnter(Collider collider)
	{
		CollisionInfo collisionInfo = new CollisionInfo();
		collisionInfo.collider = appleDestroyCollider;
		collisionInfo.otherCollider = collider;
		collisionInfo.position = collider.transform.position;
		ICollidable collidable = null;
		ICollidable[] componentsInParent = collisionInfo.otherCollider.gameObject.GetComponentsInParent<ICollidable>();
		ICollidable[] array = componentsInParent;
		foreach (ICollidable collidable2 in array)
		{
			if (collidable2.OnCollision(collisionInfo) || collidable2 is WeaponGeneral)
			{
				collidable = collidable2;
				break;
			}
		}
		if (collidable != null && collidable is WeaponGeneral)
		{
			ApplyDamage();
		}
	}

	private void DestroyApple()
	{
		if (appleDestroyEffectPrefab != null && !string.IsNullOrEmpty(appleDestroyEffectPrefab.assetGUID))
		{
			GameObject obj = Object.Instantiate(appleDestroyEffectPrefab.GetAsset(), base.transform.position, Quaternion.identity, level.FXsTransform) as GameObject;
			Object.Destroy(obj, 5f);
		}
		Object.DestroyObject(base.gameObject);
		AudioManager.PlayAudioWithKey("apple_destroy");
	}

	public void AttachedToBody(Rigidbody rigidBody)
	{
		HingeJoint component = GetComponent<HingeJoint>();
		component.connectedBody = rigidBody;
		component.autoConfigureConnectedAnchor = true;
	}

	public void DestroyJoint()
	{
		Rigidbody component = base.gameObject.GetComponent<Rigidbody>();
		HingeJoint component2 = GetComponent<HingeJoint>();
		Object.Destroy(component2);
		component.AddForce(Vector3.right * forceCoeficent);
		component.angularVelocity = new Vector3(0f - forceCoeficent / forceDivider, 0f, 0f);
	}

	public void ApplyDamage()
	{
		if (!isDestroyed)
		{
			isDestroyed = true;
			CachedManagers.CachedTutorialManager.IsAppleGameTutorialInProgress = false;
			level.IncreaseScore();
			int value = Random.Range(coinsMin, coinsMax);
			CachedManagers.CachedStatisticsManager.AddCoins(value);
			DestroyApple();
		}
	}

	public bool OnCollision(CollisionInfo collisionInfo)
	{
		OnHead = false;
		ApplyDamage();
		return false;
	}

	public void ApplyDamage(CollisionInfo collisionInfo)
	{
	}

	public void ApplyForce(float force, ForceMode forceMode, CollisionInfo collisionInfo)
	{
	}

	public virtual bool IsAffectedInRadius(Vector3 position, float radius)
	{
		bool flag = AffectedObjects(position, radius).Count > 0;
		if (flag)
		{
			ApplyDamage();
		}
		return flag;
	}

	public virtual List<Collider> AffectedObjects(Vector3 position, float radius)
	{
		affectedObjects.Clear();
		if (this != null)
		{
			List<Collider> list = new List<Collider>(GetComponents<Collider>());
			for (int i = 0; i < list.Count; i++)
			{
				Vector3 a = list[i].bounds.ClosestPoint(position);
				if (Vector3.Distance(a, position) <= radius)
				{
					affectedObjects.Add(list[i]);
					break;
				}
			}
		}
		return affectedObjects;
	}
}
