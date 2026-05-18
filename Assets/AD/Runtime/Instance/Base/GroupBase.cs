using System;
using System.Linq;
using UnityEngine;

namespace AD.Instance
{
    public abstract class GroupBase<T> : GroupBase
        where T : GroupBase<T>
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

        #endregion

        protected Canvas Canvas => GetComponentsInParent<Canvas>(true).FirstOrDefault(e => e != null);

        protected bool Displaying => gameObject.activeInHierarchy;


        private const int MaxOrder = 32767;
        private const int DefaultOrder = 10000;

        public void Show()
        {
            gameObject.SetActive(true);

            var canvas = Canvas;
            canvas.sortingOrder = MaxOrder;
            UnityEngine.Canvas.ForceUpdateCanvases();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            Canvas.sortingOrder = DefaultOrder;
        }

        private bool init;

        private void Init()
        {
            if (init) return;
            init = true;
            var _ = Instance;
            InitComponent();
            Hide();
        }

        public void Awake() => Init();

        private void OnEnable()
        {
            OnShow();
        }

        private void OnDisable()
        {
            OnHide();
        }

        protected abstract void InitComponent();

        protected virtual void OnShow()
        {
        }

        protected virtual void OnHide()
        {
        }
    }

    public abstract class GroupBase : MonoBehaviour
    {
    }
}