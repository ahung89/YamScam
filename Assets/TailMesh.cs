using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class TailMesh : MonoBehaviour {

    public int numVertsPerSide;
    public Vector2 meshDimensions;

    private Vector3[] verts;
    private Vector2[] uvs;
    private int[] triangles;

    void Awake ()
    {
        verts = new Vector3[2 * numVertsPerSide];
        uvs = new Vector2[2 * numVertsPerSide];
        triangles = new int[(numVertsPerSide - 1) * 6];

        InitMesh();
    }

    void InitMesh()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.MarkDynamic();

        float sizeIncrementY = -meshDimensions.y / numVertsPerSide;
        float uvIncrementY = 1f / (numVertsPerSide - 1);
        float width = meshDimensions.x;

        for(int i = 0; i < numVertsPerSide; i++)
        {
            verts[i * 2] = new Vector3(0, i * sizeIncrementY, 0);
            verts[i * 2 + 1] = new Vector3(width, i * sizeIncrementY, 0);
            uvs[i * 2] = new Vector2(0, 1f - i * uvIncrementY);
            uvs[i * 2 + 1] = new Vector2(1, 1f - i * uvIncrementY);

            if (i != 0)
            {
                int triIndex = (i - 1) * 2;

                // 1st triangle
                triangles[triIndex * 3] = (i - 1) * 2;
                triangles[triIndex * 3 + 1] = (i - 1) * 2 + 1;
                triangles[triIndex * 3 + 2] = i * 2;

                // 2nd triangle
                triangles[triIndex * 3 + 3] = i * 2;
                triangles[triIndex * 3 + 4] = (i - 1) * 2 + 1;
                triangles[triIndex * 3 + 5] = i * 2 + 1;
            }
        }

        mesh.vertices = verts;
        mesh.uv = uvs;
        mesh.triangles = triangles;
    }
}
