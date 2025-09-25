using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace EditorTools.Editor.AssetsProssesor
{
    public class AnimationClipProcessor : BaseEditorWindow
    {
        private List<AnimationClipInfo> animationClipInfos = new List<AnimationClipInfo>();
        private Vector2 scrollPosition;
        private string searchPath = "Assets/";
        private int filterSizeValue;

        [MenuItem("Tools/资源处理/动画剪辑处理器", priority = 101)]
        public static void ShowWindow() => ShowBigWindow<AnimationClipProcessor>();

        void OnGUI()
        {
            RenderLabel("动画剪辑收集与处理", EditorStyles.boldLabel);
            Space();

            searchPath = RenderFolderSelector("搜索路径", searchPath);
            Space();

            if (animationClipInfos != null)
            {
                RenderHorizontal(() =>
                {
                    RenderLabel("按大小过滤(kb)", GUILayout.Width(150));
                    var maxValue = animationClipInfos.Count > 0 ? animationClipInfos.First().size / 1024 : 1024;
                    filterSizeValue = RenderIntSlider(filterSizeValue, 0, maxValue);
                });
                Space();
            }

            RenderButton("收集动画剪辑", CollectAnimationClips);
            Space();

            RenderLabel(
                $"总动画数:{animationClipInfos.Count}  总大小:{EditorUtility.FormatBytes(animationClipInfos.Sum(info => info.size))}");
            Space();

            // 显示
            scrollPosition = RenderScrollView(scrollPosition, () =>
            {
                foreach (var info in animationClipInfos)
                {
                    RenderHorizontal(() =>
                    {
                        EditorGUILayout.ObjectField(info.animationClip, typeof(AnimationClip), false);
                        RenderLabel($"{EditorUtility.FormatBytes(info.size)}", GUILayout.Width(100));
                        RenderButton("减半关键帧", () => { ReduceFrameRate(info.animationClip); }, GUILayout.Width(80));
                    });
                }
            });

            // 批量处理按钮
            if (animationClipInfos.Count > 0 && GUILayout.Button("批量减半所有动画关键帧"))
            {
                if (EditorUtility.DisplayDialog("确认", $"确定要将{animationClipInfos.Count}个动画剪辑的关键帧减半吗？", "确定", "取消"))
                {
                    try
                    {
                        AssetDatabase.StartAssetEditing();
                        var index = 0;
                        foreach (var info in animationClipInfos)
                        {
                            index++;
                            ShowProgress("动画处理", $"{index}/{animationClipInfos.Count}",
                                index / (float)animationClipInfos.Count);

                            ReduceFrameRate(info.animationClip);
                        }
                    }
                    finally
                    {
                        HideProgress();
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                        AssetDatabase.StopAssetEditing();
                        Debug.Log("动画处理完成!");
                    }
                }
            }
        }

        // 收集动画
        void CollectAnimationClips()
        {
            animationClipInfos.Clear();
            string[] guids = AssetDatabase.FindAssets("t:AnimationClip", new[] { searchPath });

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
                if (clip != null)
                {
                    var info = new AnimationClipInfo(clip, GetAnimeSize(clip));
                    animationClipInfos.Add(info);
                    // Debug.Log($"{clip.name}:{EditorUtility.FormatBytes()}");
                }
            }

            animationClipInfos = animationClipInfos
                .OrderByDescending(info => info.size)
                .Where(info => info.size / 1024 > filterSizeValue)
                .ToList();

            Debug.Log($"找到 {animationClipInfos.Count} 个动画剪辑");
        }

        // 减半关键帧
        void ReduceFrameRate(AnimationClip clip)
        {
            string path = AssetDatabase.GetAssetPath(clip);
            var keyFrameCount = 0;

            EditorCurveBinding[] bindings = AnimationUtility.GetCurveBindings(clip);


            foreach (EditorCurveBinding binding in bindings)
            {
                AnimationCurve curve = AnimationUtility.GetEditorCurve(clip, binding);
                if (curve != null)
                {
                    keyFrameCount += curve.keys.Length;
                    Keyframe[] newKeys = new Keyframe[(curve.length + 1) / 2];
                    for (int i = 0, j = 0; i < curve.length; i += 2, j++)
                    {
                        newKeys[j] = curve.keys[i];
                    }

                    AnimationCurve newCurve = new AnimationCurve(newKeys);
                    AnimationUtility.SetEditorCurve(clip, binding, newCurve);
                }
            }

            EditorUtility.SetDirty(clip);
            AssetDatabase.SaveAssets();
        }

        private int GetAnimeSize(AnimationClip clip)
        {
            Type animationUtilityType = typeof(UnityEditor.AnimationUtility);
            MethodInfo getClipStatsMethod = animationUtilityType.GetMethod(
                "GetAnimationClipStats",
                BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public,
                null,
                new Type[] { typeof(AnimationClip) },
                null);

            if (getClipStatsMethod == null)
            {
                Debug.LogError("方法获取失败");
                return 0;
            }

            object clipStats = getClipStatsMethod.Invoke(null, new object[] { clip });

            if (clipStats == null)
            {
                Debug.LogError("函数调用返回空");
                return 0;
            }

            Type clipStatsType = clipStats.GetType();

            FieldInfo fieldInfo = clipStatsType.GetField("size",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);


            if (fieldInfo == null)
            {
                Debug.LogError("变量获取失败");
                return 0;
            }

            object sizeValue = fieldInfo.GetValue(clipStats);

            return (int)sizeValue;
        }
    }

    public class AnimationClipInfo
    {
        public AnimationClip animationClip;

        public int size;

        public AnimationClipInfo(AnimationClip animationClip, int size)
        {
            this.animationClip = animationClip;
            this.size = size;
        }
    }
}