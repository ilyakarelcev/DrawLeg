using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class LegsManager : MonoBehaviour
{

    [SerializeField] Section _section;
    int[] triangles;
    int createdQuads = 0;
    Mesh mesh;

    private List<Transform> _spheres = new ();
    [SerializeField] private Transform _sphereCollider;

    [SerializeField] private Leg[] _legs;

    void Start()
    {
        mesh = new Mesh();
        mesh.name = "My Mesh";
        _legs[0].MeshFilter.mesh = mesh;
        _legs[1].MeshFilter.mesh = mesh;
        //GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    public void UpdateStroke(Vector3[] positions)
    {

        if (positions.Length < 2) {
            Debug.Log("Мало точек");
            return;
        }

        mesh.Clear();

        for (int i = 0; i < _spheres.Count; i++)
        {
            Destroy(_spheres[i].gameObject);
        }
        _spheres.Clear();

        for (int l = 0; l < _legs.Length; l++)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                Transform newSphere = Instantiate(_sphereCollider, positions[i], Quaternion.identity, _legs[l].transform);
                newSphere.localPosition = positions[i];
                _spheres.Add(newSphere);
            }
        }
        

        createdQuads = 0;
        Vector3[] vertices = new Vector3[positions.Length * _section.Points.Length];
        triangles = new int[(positions.Length - 1) * (_section.Points.Length - 1) * 2 * 3];

        for (int p = 0; p < positions.Length; p++)
        {
            Quaternion rotation;
            if (p == 0)
            {
                rotation = Quaternion.LookRotation(positions[p + 1] - positions[p], Vector3.forward);
            }
            else
            {
                rotation = Quaternion.LookRotation(positions[p] - positions[p - 1], Vector3.forward);
            }

            for (int s = 0; s < _section.Points.Length; s++)
            {
                Vector3 pathPoint = positions[p];
                Matrix4x4 m = Matrix4x4.TRS(pathPoint, rotation, Vector3.one);
                Vector3 position = m.MultiplyPoint3x4(_section.Points[s].localPosition);
                vertices[s + p * _section.Points.Length] = position;
            }
        }

        for (int p = 0; p < positions.Length - 1; p++)
        {
            for (int s = 0; s < _section.Points.Length - 1; s++)
            {
                int v0 = s + p * _section.Points.Length;
                int v1 = s + 1 + p * _section.Points.Length;
                int v2 = s + 1 + (p + 1) * _section.Points.Length;
                int v3 = s + (p + 1) * _section.Points.Length;
                CreateQuad(v0, v1, v2, v3);
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

    }

    void CreateQuad(int v0, int v1, int v2, int v3)
    {
        triangles[0 + createdQuads * 6] = v3;
        triangles[1 + createdQuads * 6] = v1;
        triangles[2 + createdQuads * 6] = v0;
        triangles[3 + createdQuads * 6] = v1;
        triangles[4 + createdQuads * 6] = v3;
        triangles[5 + createdQuads * 6] = v2;
        createdQuads++;
        //Debug.Log(Mathf.Max(v0,v1,v2,v3));
    }

}
