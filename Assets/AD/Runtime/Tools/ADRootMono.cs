using UnityEngine;

namespace AD.Tools
{
    public abstract class ADRootMono<T> : MonoBehaviour where T : ADRootMono<T>
    {
        private static T instance;
        private static readonly object SyncRoot = new object();

        public static T Instance
        {
            get
            {
                lock (SyncRoot)
                {
                    if (instance != null) return instance;
                    if (!ADManagerConst.IsPlaying)
                    {
                        instance = FindObjectOfType<T>();
                        return instance;
                    }

                    if (instance == null)
                    {
                        var objects = FindObjectsOfType<T>();
                        if (objects.Length > 0)
                        {
                            instance = objects[0];
                            for (int i = 1; i < objects.Length; i++)
                            {
                                Destroy(objects[i].gameObject);
                            }
                        }

                        if (instance == null)
                        {
                            instance = ADManager.Instance.gameObject.AddComponent<T>();
                        }


                        DontDestroyOnLoad(instance.gameObject);
                    }

                    return instance;
                }
            }
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(gameObject);
                OnAwake();
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        protected abstract void OnAwake();
    }
}