using AD.Attributes;
using UnityEditor;
using UnityEngine;

namespace AD.Editor
{
    public class BasePropertyDrawer
    {
        public virtual bool OverridePropertyLayout(SerializedProperty property, in GUIContent label)
        {
            return false;
        }

        public virtual bool CanDisplay(SerializedProperty property)
        {
            return true;
        }

        public virtual void LayoutBefore(SerializedProperty property, ref GUIContent label)
        {
        }

        public virtual void LayoutAfter(SerializedProperty property, ref GUIContent label)
        {
        }

        public virtual void Before(SerializedProperty property)
        {
        }

        public virtual void After(SerializedProperty property)
        {
        }
    }
}