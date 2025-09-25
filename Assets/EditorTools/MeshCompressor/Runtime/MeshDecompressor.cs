using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

[RequireComponent(typeof(MeshFilter)),DisallowMultipleComponent]
public class MeshDecompressor : MonoBehaviour
{
    public CompressedMeshData compressedData;
    [FormerlySerializedAs("material")] public Material[] materials;

    void Start()
    {
        if (compressedData == null) return;
        
        Mesh mesh = DecompressMesh();
        GetComponent<MeshFilter>().mesh = mesh;
        if (materials != null)
            GetComponent<MeshRenderer>().materials = materials;
    }

    public Mesh DecompressMesh()
    {
        Mesh mesh = new Mesh();
        mesh.name = "Decompressed Mesh";

        // 解压顶点
        float[] vertexFloats = HalfPrecision.HalfToFloat(compressedData.compressedVertices);
        Vector3[] vertices = new Vector3[vertexFloats.Length / 3];
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(
                vertexFloats[i * 3],
                vertexFloats[i * 3 + 1],
                vertexFloats[i * 3 + 2]
            );
        }
        mesh.vertices = vertices;
        mesh.bounds = compressedData.bounds;

        // 解压三角形
        List<int> triangles = new List<int>();
        if (compressedData.use16BitIndex)
        {
            for (int i = 0; i < compressedData.compressedTriangles.Length; i += 2)
            {
                triangles.Add((compressedData.compressedTriangles[i + 1] << 8) | 
                              compressedData.compressedTriangles[i]);
            }
        }
        else
        {
            for (int i = 0; i < compressedData.compressedTriangles.Length; i += 4)
            {
                triangles.Add(
                    (compressedData.compressedTriangles[i + 3] << 24) |
                    (compressedData.compressedTriangles[i + 2] << 16) |
                    (compressedData.compressedTriangles[i + 1] << 8) |
                    compressedData.compressedTriangles[i]
                );
            }
        }
        mesh.SetTriangles(triangles, 0);

        // 重新计算法线和切线
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        mesh.RecalculateBounds();

        return mesh;
    }
}