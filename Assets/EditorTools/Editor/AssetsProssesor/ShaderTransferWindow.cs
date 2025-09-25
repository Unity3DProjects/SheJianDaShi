using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EditorTools.Editor.AssetsProssesor
{
    public class ShaderTransferWindow : BaseEditorWindow
    {
        [MenuItem("Tools/资源处理/材质处理器", priority = 102)]
        public static void ShowWindow() => ShowVerticalWindow<ShaderTransferWindow>("材质处理器", 550f, 800f);

        private Shader oldShader;
        private Shader newShader;

        private List<TransferPair> transferPairs = new List<TransferPair>();

        private Vector2 pos1;
        private Vector2 pos2;
        private Vector2 pos3;
        private bool isUseShaderFind;
        private string newShaderPath = "Universal Render Pipeline/Lit";

        void OnGUI()
        {
            var selectedMats = Selection.GetFiltered<Material>(SelectionMode.Assets);
            oldShader = selectedMats != null && selectedMats.Length > 0 ? selectedMats.First()?.shader : oldShader;
            Space();
            isUseShaderFind = RenderToggle("是否启动Shader.Find查找", isUseShaderFind);
            oldShader = oldShader.Render("旧 Shader", typeof(Shader)) as Shader;
            if (isUseShaderFind)
            {
                newShaderPath = RenderTextField("新 shader路径：", newShaderPath);
                newShader = newShader.Render("新 Shader", typeof(Shader)) as Shader;
                newShader = Shader.Find(newShaderPath);
            }
            else
            {
                newShader = newShader.Render("新 Shader", typeof(Shader)) as Shader;
            }

            Space();
            RenderLine(Color.cyan);
            Space();
            RenderHorizontal(() =>
            {
                RenderButton("添加属性", () => { transferPairs.Add(new TransferPair()); });
                RenderButton("删除属性", () => { transferPairs.Remove(transferPairs.Last()); });
            });
            pos2 = RenderScrollView(pos2, () =>
            {
                foreach (var pair in transferPairs)
                {
                    RenderTextField("旧 属性名", ref pair.oldPropName);
                    RenderTextField("新 属性名", ref pair.newPropName);
                    pair.propertyType = (PropertyType)RenderEnumPopup("属性类型", pair.propertyType);
                    Space();
                    RenderLine();
                }
            }, Height(150));

            Space(10);
            RenderLine(Color.cyan);
            Space(10);

            pos3 = RenderScrollView(pos3, () =>
            {
                RenderTextField("Standard 主贴图", "_MainTex");
                RenderTextField("Standard 颜色", "_Color");
                RenderLine();
                RenderTextField("URP-Lit 主贴图", "_BaseMap");
                RenderTextField("URP-Lit 颜色", "_BaseColor");
            }, Height(150));

            Space(10);
            RenderLine(Color.cyan);
            Space(10);

            pos1 = RenderScrollView(pos1, () =>
            {
                var mats = this.FindAndLoadAssetAtPath<Material>("t:Material", new[] { "Assets" })
                    .Where(mat => { return mat != null && mat.shader == oldShader; }).ToArray();
                if (mats != null)
                {
                    foreach (var material in mats)
                    {
                        RenderHorizontal(() =>
                        {
                            material.Render(typeof(Material), false);
                            RenderButton("替换Shader", () => { ProcessMaterials(material); }, GUILayout.Width(100f));
                        });
                    }
                }
            });

            RenderLine(Color.cyan);
            Space();
            RenderHorizontal(() =>
            {
                RenderButton("转换选中材质", () => { TransferSelectedMaterials(); });

                RenderButton("转换所有需替换材质", () =>
                {
                    if (EditorUtility.DisplayDialog("确认更换?",
                            "这会更换所有old shader所在的材质",
                            "是", "否"))
                    {
                        TransferAllProjectMaterials();
                    }
                });
            });
            Space();
        }

        void TransferSelectedMaterials()
        {
            Material[] mats = Selection.GetFiltered<Material>(SelectionMode.Assets);
            ProcessMaterials(mats);
        }

        void TransferAllProjectMaterials()
        {
            var mats = this.FindAndLoadAssetAtPath<Material>("t:Material", new[] { "Assets" })
                .Where(mat => { return mat != null && mat.shader == oldShader; }).ToArray();
            ProcessMaterials(mats);
        }

        void ProcessMaterials(params Material[] mats)
        {
            if (newShader == null || mats == null)
            {
                Debug.Log("替换shader或材质不能为空");
                return;
            }

            if (transferPairs == null || transferPairs.Count <= 0) return;

            int processedCount = 0;

            foreach (var mat in mats)
            {
                HandleTransfer(mat);
                processedCount++;
                EditorUtility.SetDirty(mat);
            }

            AssetDatabase.SaveAssets();
            EditorUtility.DisplayDialog("处理完成",
                $"处理了 {processedCount} 材质.", "OK");
        }

        private void HandleTransfer(Material mat)
        {
            if (mat.shader != oldShader) return;
            List<(bool, string, Texture)> textures = new List<(bool, string, Texture)>();
            List<(bool, string, Color)> colors = new List<(bool, string, Color)>();

            foreach (var pair in transferPairs)
            {
                switch (pair.propertyType)
                {
                    case PropertyType.Texture:
                        bool haveProperty = mat.HasProperty(pair.oldPropName);
                        Texture tex = mat.GetTexture(pair.oldPropName);
                        textures.Add((haveProperty, pair.newPropName, tex));
                        break;
                    case PropertyType.Color:
                        bool haveProperty2 = mat.HasProperty(pair.oldPropName);
                        Color color = mat.GetColor(pair.oldPropName);
                        colors.Add((haveProperty2, pair.newPropName, color));
                        break;
                }
            }

            mat.shader = newShader;

            foreach (var t in textures)
            {
                if (t.Item3 != null && mat.HasProperty(t.Item2))
                {
                    mat.SetTexture(t.Item2, t.Item3);
                }
            }

            foreach (var t in colors)
            {
                if (t.Item3 != null && mat.HasProperty(t.Item2))
                {
                    mat.SetColor(t.Item2, t.Item3);
                }
            }
        }
    }

    public class TransferPair
    {
        public string oldPropName;
        public string newPropName;
        public PropertyType propertyType;
    }

    public enum PropertyType
    {
        Texture,
        Color,
    }
}