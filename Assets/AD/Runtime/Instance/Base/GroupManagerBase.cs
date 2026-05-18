using UnityEngine;

namespace AD.Instance
{
    public abstract class GroupManagerBase<T, TGroup> : MonoBehaviour
        where TGroup : GroupBase<TGroup>
        where T : GroupManagerBase<T, TGroup>
    {
        #region Instance

        private static T instance;
        private static readonly object lockObj = new object();

        public static T Instance
        {
            get
            {
                lock (lockObj)
                {
                    if (instance != null) return instance;
                    if (!ADManagerConst.IsPlaying)
                    {
                        instance = FindObjectOfType<T>();
                        return instance;
                    }

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
                            obj.name = typeof(T).Name;
                            instance = obj.AddComponent<T>();
                        }

                        DontDestroyOnLoad(instance.transform.root.gameObject);
                        instance.Init();
                    }

                    return instance;
                }
            }
        }

        protected static TGroup GroupInstance => GroupBase<TGroup>.Instance;

        #endregion
        private bool init;

        private void Init()
        {
            if (init) return;
            init = true;
            var _ = Instance;
            var __ = GroupInstance;
            InitManager();
        }

        public void Awake() => Init();

        protected virtual void InitManager()
        {
            GroupInstance.Hide();
        }
    }
}