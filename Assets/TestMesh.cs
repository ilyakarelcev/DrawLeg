using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMesh : MonoBehaviour
{

    [SerializeField] private MeshFilter _meshFilter;


    void Start()
    {
        
        Mesh mesh = new Mesh();

        Vector3[] vertexes = new Vector3[4];
        vertexes[0] = new Vector3(0, 0, 0);
        vertexes[1] = new Vector3(1, 0, 0);
        vertexes[2] = new Vector3(1, 1, 0);
        vertexes[3] = new Vector3(0, 1, 0);

        int[] triangles = new int[6];
        triangles[0] = 0;
        triangles[1] = 2;
        triangles[2] = 1;

        triangles[3] = 0;
        triangles[4] = 3;
        triangles[5] = 2;

        mesh.vertices= vertexes;
        mesh.triangles= triangles;

        _meshFilter.mesh = mesh;

    }

}
