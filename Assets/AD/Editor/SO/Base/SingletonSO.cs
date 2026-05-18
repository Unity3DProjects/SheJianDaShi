using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AD.Editor
{
    public abstract class SingletonSO<T> : ScriptableObject
        where T : SingletonSO<T>
    {
        protected static readonly string AssetPath = $"{ADEditorConst.EditorSORoot}/{typeof(T).Name}.asset";

        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance != null) return instance;
                instance = AssetDatabase.LoadAssetAtPath<T>(AssetPath);
                if (instance == null)
                {
                    Mkdir(Path.GetDirectoryName(AssetPath));
                    AssetDatabase.CreateAsset(CreateInstance<T>(), AssetPath);
                    AssetDatabase.ForceReserializeAssets(new string[] {AssetPath});
                    instance = AssetDatabase.LoadAssetAtPath<T>(AssetPath);
                    instance.AfterCreate();
                    if (instance == null) throw new Exception("Create Asset Failure");
                }

                return instance;
            }
        }

        protected static void Mkdir(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                AssetDatabase.Refresh();
            }
        }

        protected virtual void AfterCreate(){}
    }
}