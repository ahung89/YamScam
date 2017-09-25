using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class TailMesh : MonoBehaviour {

    public int numVertsPerSection;
    public Vector2 meshDimensions;
    public List<Transform> tailSectionTransforms;
    public bool drawGizmos;

    private Vector3[] verts;
    private Vector2[] uvs;
    private int[] triangles;

    private List<Vector3> tailCenters;
    private List<SplinePoint> splinePoints;
    private Mesh mesh;
    private int numVertsPerSide;

    void Awake ()
    {
        numVertsPerSide = numVertsPerSection * (tailSectionTransforms.Count - 1);
        verts = new Vector3[2 * numVertsPerSide];
        uvs = new Vector2[2 * numVertsPerSide];
        triangles = new int[(numVertsPerSide - 1) * 6];
        tailCenters = new List<Vector3>();

        InitMesh();
    }

    private void Update ()
    {
        RecalculateVerts();
    }

    void BuildSpline()
    {
        tailCenters.Clear();
        foreach (Transform t in tailSectionTransforms)
        {
            tailCenters.Add(t.position);
        }

        splinePoints = SplineUtil.GenerateSpline(tailCenters, numVertsPerSection);
    }

    void InitMesh ()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.MarkDynamic();

        float uvIncrementY = 1f / (numVertsPerSide - 1);
        float width = meshDimensions.x;

        RecalculateVerts();

        for (int i = 0; i < numVertsPerSide; i++)
        {
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

        mesh.uv = uvs;
        mesh.triangles = triangles;
    }

    void RecalculateVerts()
    {
        BuildSpline();

        float tangentScale = meshDimensions.x / 2f;

        for (int i = 0; i < numVertsPerSide; i++)
        {
            Vector2 transformedPoint = transform.InverseTransformPoint(splinePoints[i].point);
            Vector2 transformedTangent = transform.InverseTransformVector(splinePoints[i].tangent);

            verts[i * 2] = transformedPoint - transformedTangent * tangentScale;
            verts[i * 2 + 1] = transformedPoint + transformedTangent * tangentScale;
        }

        mesh.vertices = verts;
    }

    private void OnDrawGizmos ()
    {
        if (drawGizmos)
        {
            Gizmos.color = Color.green;

            if (splinePoints != null)
            {
                foreach (SplinePoint point in splinePoints)
                {
                    Gizmos.DrawLine(point.point - .15f * point.tangent, point.point + .15f * point.tangent);
                }
            }
        }
    }
}
