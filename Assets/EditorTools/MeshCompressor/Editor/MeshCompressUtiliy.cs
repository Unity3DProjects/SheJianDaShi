using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace MeshCompressor.Runtime.Utility
{
    public static class MeshCompressUtiliy
    {
        public static CompressedMeshData CompressMesh(string path)
        {
            var mesh = AssetDatabase.LoadAssetAtPath<Mesh>(path);
            CompressedMeshData data = ScriptableObject.CreateInstance<CompressedMeshData>();
            data.bounds = mesh.bounds;

            // 压缩顶点
            Vector3[] vertices = mesh.vertices;
            float[] vertexFloats = new float[vertices.Length * 3];
            for (int i = 0; i < vertices.Length; i++)
            {
                vertexFloats[i * 3] = vertices[i].x;
                vertexFloats[i * 3 + 1] = vertices[i].y;
                vertexFloats[i * 3 + 2] = vertices[i].z;
            }

            data.compressedVertices = HalfPrecision.FloatToHalf(vertexFloats);

            // 压缩三角形
            int[] triangles = mesh.triangles;
            bool use16Bit = triangles.Length < 65535;
            data.use16BitIndex = use16Bit;

            if (use16Bit)
            {
                data.compressedTriangles = new byte[triangles.Length * 2];
                for (int i = 0; i < triangles.Length; i++)
                {
                    data.compressedTriangles[i * 2] = (byte)(triangles[i] & 0xFF);
                    data.compressedTriangles[i * 2 + 1] = (byte)(triangles[i] >> 8);
                }
            }
            else
            {
                data.compressedTriangles = new byte[triangles.Length * 4];
                for (int i = 0; i < triangles.Length; i++)
                {
                    data.compressedTriangles[i * 4] = (byte)(triangles[i] & 0xFF);
                    data.compressedTriangles[i * 4 + 1] = (byte)((triangles[i] >> 8) & 0xFF);
                    data.compressedTriangles[i * 4 + 2] = (byte)((triangles[i] >> 16) & 0xFF);
                    data.compressedTriangles[i * 4 + 3] = (byte)((triangles[i] >> 24) & 0xFF);
                }
            }


            var targetPath = Path.Combine(Path.GetDirectoryName(path),
                Path.GetFileNameWithoutExtension(path) + "_Compressed.asset");
            AssetDatabase.CreateAsset(data, targetPath);
            AssetDatabase.SaveAssets();
            return AssetDatabase.LoadAssetAtPath<CompressedMeshData>(targetPath);
        }

        public static CompressedMeshData CompressMesh(Mesh mesh)
        {
            CompressedMeshData data = ScriptableObject.CreateInstance<CompressedMeshData>();
            data.bounds = mesh.bounds;

            // 压缩顶点
            Vector3[] vertices = mesh.vertices;
            float[] vertexFloats = new float[vertices.Length * 3];
            for (int i = 0; i < vertices.Length; i++)
            {
                vertexFloats[i * 3] = vertices[i].x;
                vertexFloats[i * 3 + 1] = vertices[i].y;
                vertexFloats[i * 3 + 2] = vertices[i].z;
            }

            data.compressedVertices = HalfPrecision.FloatToHalf(vertexFloats);

            // 压缩三角形
            int[] triangles = mesh.triangles;
            bool use16Bit = triangles.Length < 65535;
            data.use16BitIndex = use16Bit;

            if (use16Bit)
            {
                data.compressedTriangles = new byte[triangles.Length * 2];
                for (int i = 0; i < triangles.Length; i++)
                {
                    data.compressedTriangles[i * 2] = (byte)(triangles[i] & 0xFF);
                    data.compressedTriangles[i * 2 + 1] = (byte)(triangles[i] >> 8);
                }
            }
            else
            {
                data.compressedTriangles = new byte[triangles.Length * 4];
                for (int i = 0; i < triangles.Length; i++)
                {
                    data.compressedTriangles[i * 4] = (byte)(triangles[i] & 0xFF);
                    data.compressedTriangles[i * 4 + 1] = (byte)((triangles[i] >> 8) & 0xFF);
                    data.compressedTriangles[i * 4 + 2] = (byte)((triangles[i] >> 16) & 0xFF);
                    data.compressedTriangles[i * 4 + 3] = (byte)((triangles[i] >> 24) & 0xFF);
                }
            }

            var dir = @"Assets/CompressedMesh";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var targetPath = Path.Combine(dir,
                mesh.name + "_Compressed.asset");
            AssetDatabase.CreateAsset(data, targetPath);
            AssetDatabase.SaveAssets();
            return AssetDatabase.LoadAssetAtPath<CompressedMeshData>(targetPath);
        }


        public static void CompressMeshs(string[] paths)
        {
            try
            {
                AssetDatabase.StopAssetEditing();
                foreach (var path in paths)
                {
                    CompressMesh(path);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
    }
}