using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chunk : MonoBehaviour
{
    public int chunkSizeX = 16;
    public int chunkSizeY = 16;
    public int chunkSizeZ = 16;
    public int chunkX;
    public int chunkZ;
    public bool updateNeeded;

    public WorldGen World;
    private List<Vector3> newVertices = new List<Vector3>();
    private List<int> newTriangles = new List<int>();
    private List<Vector2> newUV = new List<Vector2>();

    private Mesh mesh;
    private MeshCollider coll;
    private Cube[, ,] cubes;
    private int faceCount;
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        coll = GetComponent<MeshCollider>();
        GenerateMesh();
    }

    public void CreateChunk(int posX, int posZ, int sizeX, int sizeY, int sizeZ)
    {
        this.chunkX = posX;
        this.chunkZ = posZ;

        this.chunkSizeX = sizeX;
        this.chunkSizeY = sizeY;
        this.chunkSizeZ = sizeZ;

        cubes = new Cube[chunkSizeX, chunkSizeY, chunkSizeY];

        for (int x = 0; x < chunkSizeX; x++)
        {
            for (int y = 0; y < chunkSizeY; y++)
            {
                for (int z = 0; z < chunkSizeZ; z++)
                {
                    cubes[x, y, z] = new Cube(this, x, y, z, Block(x, y, z));
                }
            }
        }
    }

    public byte Block(int x, int y, int z)
    {
        return World.Block(x + chunkX, y, z + chunkZ);
    }

    void GenerateMesh()
    {
        for (int x = 0; x < chunkSizeX; x++)
        {
            for (int y = 0; y < chunkSizeY; y++)
            {
                for (int z = 0; z < chunkSizeZ; z++)
                {
                    //This code will run for every block in the chunk

                    if (Block(x, y, z) != 0)
                    {
                        //If the block is solid
                        faceCount = cubes[x, y, z].DrawCube(newVertices, newTriangles, newUV, faceCount);
                    }
                }
            }
        }

        UpdateMesh();
    }

    void LateUpdate()
    {
        if (updateNeeded)
        {
            GenerateMesh();
            updateNeeded = false;
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = newVertices.ToArray();
        mesh.uv = newUV.ToArray();
        mesh.triangles = newTriangles.ToArray();
        mesh.Optimize();
        mesh.RecalculateNormals();

        coll.sharedMesh = null;
        coll.sharedMesh = mesh;

        newVertices.Clear();
        newUV.Clear();
        newTriangles.Clear();

        faceCount = 0;
    }
}
