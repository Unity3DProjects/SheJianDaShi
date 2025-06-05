using UnityEngine;

namespace Soomla.Traceback.Singletons
{
	public abstract class BaseBehaviour : MonoBehaviour
	{
		private Transform cashedTransform;

		public Transform CachedTransform
		{
			get
			{
				return cashedTransform ?? (cashedTransform = base.transform);
			}
		}

		protected virtual void Awake()
		{
		}

		protected virtual void Start()
		{
		}

		protected virtual void OnDestroy()
		{
		}
	}
}
