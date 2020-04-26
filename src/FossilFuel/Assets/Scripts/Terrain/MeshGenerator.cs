using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    private Mesh terrainMesh;
    private MeshFilter mFilter;
    private MeshCollider mColl;

    [SerializeField]
    Material meshMaterial;

    [SerializeField, Tooltip("The minimum and maximum physical bounds of the terrain mesh.")]
    private float xMin, xMax, zMin, zMax;

    [SerializeField, Tooltip("The number of vertices to draw in the x direction.")]
    private int xVertices = 50;
    [SerializeField, Tooltip("The number of vertices to draw in the z direction.")]
    private int zVertices = 50;

    private Vector3[] verticesList;
    private Vector3[,] verts2DList;

    private int[] trisList;

    private List<Vector3> vertsToMove;

    Matrix4x4 localToWorld;

    // Start is called before the first frame update
    void Start()
    {
        terrainMesh = new Mesh();

        vertsToMove = new List<Vector3>();

        //mFilter = this.gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
        mFilter = this.gameObject.GetComponent<MeshFilter>();

        mColl = this.gameObject.GetComponent<MeshCollider>();

        GetComponent<MeshRenderer>().material = meshMaterial;

        mFilter.mesh = terrainMesh;

        GenerateTerrain();

        UpdateTerrainMesh();

        localToWorld = transform.localToWorldMatrix;
    }

    private void GenerateTerrain()
    {
        verticesList = new Vector3[(xVertices + 1) * (zVertices + 1)];
        verts2DList = new Vector3[xVertices + 1, zVertices + 1];

        float prevYVal = 0.75f;
        int i = 0;
        for (float z = 0; z <= zVertices; z++)
        {
            for (float x = 0; x <= xVertices; x++)
            {
                int choice = UnityEngine.Random.Range(0, 2);

                if (z > 0 && x > 0) prevYVal = choice == 0 ? verts2DList[(int)x, (int)z - 1].y : verts2DList[(int)x - 1, (int)z].y;
                else if (z > 0 && x <= 0) prevYVal = verts2DList[(int)x, (int)z - 1].y;
                else if (x > 0 && z <= 0) prevYVal = verts2DList[(int)x - 1, (int)z].y;

                float yVal = prevYVal + UnityEngine.Random.Range(-0.1f, 0.1f);
                yVal = Mathf.Clamp(yVal, .5f, 3f);
                //prevYVal = yVal;

                verticesList[i] = new Vector3(xMin + (x / xVertices * (xMax - xMin)), yVal, zMin + (z / zVertices * (zMax - zMin)));

                verts2DList[(int)x, (int)z] = verticesList[i];

                i++;
            }
        }

        trisList = new int[xVertices * zVertices * 6];

        int vert = 0;
        int tris = 0;
        for (int z = 0; z < zVertices; z++)
        {
            for (int x = 0; x < xVertices; x++)
            {
                trisList[tris + 0] = vert + 0;
                trisList[tris + 1] = vert + xVertices + 1;
                trisList[tris + 2] = vert + 1;
                trisList[tris + 3] = vert + 1;
                trisList[tris + 4] = vert + xVertices + 1;
                trisList[tris + 5] = vert + xVertices + 2;

                vert++;
                tris += 6;
            }

            vert++;
        }
    }
    
    private void UpdateTerrainMesh()
    {
        terrainMesh.Clear();

        terrainMesh.vertices = verticesList;
        terrainMesh.triangles = trisList;

        terrainMesh.RecalculateNormals();
        terrainMesh.RecalculateBounds();

        // Apply the terrain mesh to the mesh collider AFTER recalculating bounds
        mColl.sharedMesh = terrainMesh;

        localToWorld = transform.localToWorldMatrix;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Explosion")
        {
            //vertsToMove = new List<Vector3>();

            for(int v = 0; v < verticesList.Length; v++)
            {
                while (Vector3.Distance(other.transform.position, localToWorld.MultiplyPoint3x4(verticesList[v])) < other.bounds.extents.magnitude * 0.75f)
                {
                    verticesList[v] += Vector3.down * 0.05f;
                }
            }

            UpdateTerrainMesh();
        }
    }

    //private void OnDrawGizmos()
    //{
    //    if (verticesList == null)
    //        return;

    //    for (int i = 0; i < verticesList.Length; i++)
    //    {
    //        Gizmos.DrawSphere(verticesList[i], 0.1f);
    //    }
    //}
}
