using UnityEngine;

namespace AD.Runtime.Tools.Tools
{
    public class SingletonMonoBase<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        private static readonly object lockObj = new object();

        public static T Instance
        {
            get
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        instance = FindObjectOfType<T>();

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
                        }

                        if (instance == null)
                        {
                            var obj = new GameObject();
                            obj.name = typeof(T).ToString();
                            instance = obj.AddComponent<T>();
                            DontDestroyOnLoad(obj);
                        }
                        else
                        {
                            DontDestroyOnLoad(instance.gameObject);
                        }
                    }

                    return instance;
                }
            }
        }

        public virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }
        }
    }
}