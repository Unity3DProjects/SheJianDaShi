using System.Collections.Generic;
using MeshCompressor.Runtime.Utility;
using UnityEditor;
using UnityEngine;

namespace EditorTools.Editor.ComponentModifyer
{
    public class MeshModifyer
    {
        private static Dictionary<string, CompressedMeshData> compressedMeshDataDic =
            new Dictionary<string, CompressedMeshData>();

        [MenuItem("Tools/组件修改/压缩所有网格", priority = 204)]
        public static void CompressAndModifyAllMesh()
        {
            compressedMeshDataDic.Clear();
            EditorHelper.ModifyAllComponents<MeshFilter>(meshFilter =>
            {
                if (meshFilter.sharedMesh == null) return;
                var renderer = meshFilter.GetComponent<MeshRenderer>();
                var meshPath = AssetDatabase.GetAssetPath(meshFilter.sharedMesh);

                if (!compressedMeshDataDic.TryGetValue(meshFilter.sharedMesh.name, out var data))
                {
                    data = MeshCompressUtiliy.CompressMesh(meshFilter.sharedMesh);
                    compressedMeshDataDic[meshFilter.sharedMesh.name] = data;
                }

                if (data == null)
                {
                    Debug.LogError($"{meshPath}:压缩失败！");
                }

                var meshDecompressor = meshFilter.gameObject.GetComponent<MeshDecompressor>();
                if (meshDecompressor == null)
                {
                    meshDecompressor = meshFilter.gameObject.AddComponent<MeshDecompressor>();
                }

                meshDecompressor.compressedData = data;
                meshFilter.mesh = null;
                if (renderer)
                {
                    List<Material> projectMatilrals = new List<Material>();
                    foreach (var rendererMaterial in renderer.sharedMaterials)
                    {
                        var path = AssetDatabase.GetAssetPath(rendererMaterial);
                        projectMatilrals.Add(AssetDatabase.LoadAssetAtPath<Material>(path));
                    }

                    meshDecompressor.materials = projectMatilrals.ToArray();
                }

                EditorUtility.SetDirty(meshFilter.gameObject);
            });
        }
    }
}