using UnityEngine;

[CreateAssetMenu(menuName = "Mesh Data/Compressed Mesh", fileName = "compressed_mesh.asset")]
public class CompressedMeshData : ScriptableObject
{
    [HideInInspector] public byte[] compressedVertices;
    [HideInInspector] public byte[] compressedTriangles;
    [HideInInspector] public Bounds bounds;
    [HideInInspector] public bool use16BitIndex;
}