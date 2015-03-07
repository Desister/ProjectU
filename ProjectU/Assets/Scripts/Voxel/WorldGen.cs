using UnityEngine;
using System.Collections;

public class WorldGen : MonoBehaviour
{
    public GameObject chunk;
    public Chunk[,] chunks;
    public int chunkSizeX = 16;
    public int chunkSizeY = 16;
    public int chunkSizeZ = 16;

    private int worldX = 64;
    private int worldY = 16;
    private int worldZ = 64;

    public byte[, ,] data;

    // Use this for initialization
    void Start()
    {
        data = new byte[worldX, worldY, worldZ];

        for (int x = 0; x < worldX; x++)
        {
            for (int z = 0; z < worldZ; z++)
            {
                int stone = PerlinNoise(x, 0, z, 10, 3, 1.2f);
                stone += PerlinNoise(x, 300, z, 20, 4, 0) + 10;
                int dirt = PerlinNoise(x, 100, z, 50, 2, 0) + 1; //Added +1 to make sure minimum grass height is 1

                for (int y = 0; y < worldY; y++)
                {
                    if (y <= stone)
                    {
                        data[x, y, z] = 1;
                    }
                    else if (y <= dirt + stone)
                    {
                        data[x, y, z] = 2;
                    }

                }
            }
        }

        chunks = new Chunk[Mathf.FloorToInt(worldX / chunkSizeX),
                                 Mathf.FloorToInt(worldZ / chunkSizeZ)];

    }

    public void GenColumn(int x, int z)
    {
        GameObject newChunkGO = Instantiate(chunk,
         new Vector3(x * chunkSizeX - 0.5f, 0 * chunkSizeY + 0.5f, z * chunkSizeZ - 0.5f),
         new Quaternion(0, 0, 0, 0)) as GameObject;

        var newChunk = newChunkGO.GetComponent<Chunk>();
        newChunk.World = this;
        chunks[x, z] = newChunk;
        newChunk.CreateChunk(x * chunkSizeX, z * chunkSizeZ, chunkSizeX, chunkSizeY, chunkSizeZ);
    }

    public void UnloadColumn(int x, int z)
    {
        Object.Destroy(chunks[x, z].gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public byte Block(int x, int y, int z)
    {

        if (x >= worldX || x < 0 || y >= worldY || y < 0 || z >= worldZ || z < 0)
        {
            return (byte)0;
        }

        return data[x, y, z];
    }

    int PerlinNoise(int x, int y, int z, float scale, float height, float power)
    {
        float rValue;
        rValue = Noise.GetNoise(((double)x) / scale, ((double)y) / scale, ((double)z) / scale);
        rValue *= height;

        if (power != 0)
        {
            rValue = Mathf.Pow(rValue, power);
        }

        return (int)rValue;
    }
}
