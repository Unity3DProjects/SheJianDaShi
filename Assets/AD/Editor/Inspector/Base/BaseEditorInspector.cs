using UnityEditor;
using UnityEngine;

namespace AD.Editor
{
    internal abstract partial class BaseEditorInspector : UnityEditor.Editor
    {
        public sealed override void OnInspectorGUI()
        {
            var b = GUI.enabled;
            if (EditorApplication.isCompiling)
            {
                GUI.enabled = false;
                GUILayout.BeginHorizontal();
                GUILayout.Label("编译中...");
                GUILayout.EndHorizontal();
            }

            try
            {
                if (SceneOnly && !IsInspectorInScene())
                {
                    var notSceneLabel = new GUIStyle(GUI.skin.label)
                    {
                        fontSize = 18,
                        fontStyle = FontStyle.Bold
                    };
                    Module(() => { Label("在场景中使用", notSceneLabel); }, ColorR(new Color(1, 0.5f, 0.4f, 1)));
                    return;
                }

                Layout();
            }
            finally
            {
                GUI.enabled = b;
            }
        }
        
        private static bool IsInspectorInScene()
        {
            var go = Selection.activeGameObject;
            if (go == null) return false;
            var prefabType = PrefabUtility.GetPrefabAssetType(go);
            var instanceStatus = PrefabUtility.GetPrefabInstanceStatus(go);
            return prefabType != PrefabAssetType.NotAPrefab && instanceStatus != PrefabInstanceStatus.NotAPrefab;
        }

        protected void DrawBaseInspector() => base.OnInspectorGUI();
        protected virtual bool SceneOnly => false;
        protected abstract void Layout();
    }
}