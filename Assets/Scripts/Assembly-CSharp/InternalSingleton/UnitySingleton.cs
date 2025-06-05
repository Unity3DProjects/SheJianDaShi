using System;
using UnityEngine;

namespace InternalSingleton
{
	public class UnitySingleton<T> where T : MonoBehaviour
	{
		private static T instanceT;

		internal static Type typeT = typeof(T);

		private static int globalInstanceCount;

		private static bool destroySingletonCalled;

		private static readonly string autocreateRooName = "AutoSingletons";

		private static Transform autoCreateRoot;

		private static Transform AutocreateRoot
		{
			get
			{
				if (autoCreateRoot == null)
				{
					GameObject gameObject = GameObject.Find(autocreateRooName);
					if (gameObject == null)
					{
						gameObject = new GameObject(autocreateRooName);
						UnityEngine.Object.DontDestroyOnLoad(gameObject);
					}
					autoCreateRoot = gameObject.transform;
				}
				return autoCreateRoot;
			}
		}

		private UnitySingleton()
		{
		}

		internal static void InternalAwake(T instance)
		{
			globalInstanceCount++;
			if (globalInstanceCount > 1)
			{
				CustomDebug.LogWarning("More than one awake of SingletonMonoBehaviour " + typeof(T).Name);
			}
			else
			{
				instanceT = instance;
			}
		}

		internal static void InternalDestroy()
		{
			if (globalInstanceCount > 0)
			{
				globalInstanceCount--;
				if (globalInstanceCount == 0)
				{
					destroySingletonCalled = true;
					instanceT = (T)null;
				}
			}
			else
			{
				CustomDebug.LogWarning("More than one destroy of SingletonMonoBehaviour " + typeof(T).Name);
			}
		}

		public static T GetSingleton(bool autoCreate)
		{
			if (!instanceT)
			{
				T val = (T)null;
				if (!val)
				{
					if (autoCreate && !destroySingletonCalled)
					{
						GameObject gameObject = new GameObject(typeT.Name, typeT);
						gameObject.transform.parent = AutocreateRoot;
						CustomDebug.LogWarning("Create singleton : " + typeT.Name);
						val = gameObject.GetComponent<T>();
						if (val == null)
						{
							CustomDebug.LogError("Auto created object does not have component " + typeT.Name);
							val = (T)null;
						}
					}
					else
					{
						val = (T)null;
					}
				}
				instanceT = val;
			}
			return instanceT;
		}
	}
}
