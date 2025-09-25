using TMPro;
using UnityEditor;
using UnityEngine;

namespace EditorTools.Editor.ComponentModifyer
{
    public class TMPSpriteModifyWindow : EditorWindow
    {
        private static TMP_SpriteAsset _spriteAsset;

        [MenuItem("Tools/组件修改/TextMeshPro图片资产修复",priority = 201)]
        private static void CreateWindow()
        {
            var window = EditorWindow.CreateWindow<TMPSpriteModifyWindow>();

            window.position = new Rect(Screen.width / 4f, Screen.height / 5f, 400f, 300f);
        }

        private void OnGUI()
        {
            GUILayout.Space(10);
            _spriteAsset =
                EditorGUILayout.ObjectField("选择图片资产", _spriteAsset, typeof(TMP_SpriteAsset), false) as TMP_SpriteAsset;

            GUILayout.Space(20);
            if (GUILayout.Button("修改图片资产"))
            {
                ModifySpriteAsset();
            }
        }


        public static void ModifySpriteAsset()
        {
            if (_spriteAsset == null)
            {
                Debug.Log($"{nameof(TMP_SpriteAsset)}为空!");
                return;
            }

            EditorHelper.ModifyAllComponents<TMP_Text>(tmp_Text => { tmp_Text.spriteAsset = _spriteAsset; });
        }
    }
}