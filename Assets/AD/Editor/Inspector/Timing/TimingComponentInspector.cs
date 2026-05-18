using UnityEditor;
using UnityEngine;

namespace AD.Editor
{
    [CustomEditor(typeof(TimingComponent), true), CanEditMultipleObjects]
    internal class TimingComponentInspector : BaseEditorInspector
    {
        protected override void Layout()
        {
            GUILayout.Label("仅在运行时预览");
            using (new EditorGUI.DisabledScope(true))
            {
                DrawBaseInspector();
            }
        }
    }
}