using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cube
{
    private float tUnit = 0.25f;

    private Vector2 tStone = new Vector2(1, 0);
    private Vector2 tGrass = new Vector2(0, 1);
    private Vector2 tGrassTop = new Vector2(1, 1);

    private Chunk chunk;
    private byte type;
    private int x;
    private int y;
    private int z;

    public Cube(Chunk chunk, int x, int y, int z, byte type)
    {
        this.chunk = chunk;
        this.x = x;
        this.y = y;
        this.z = z;
        this.type = type;
    }

    void CubeTop(List<Vector3> vertices)
    {
        vertices.Add(new Vector3(x, y, z + 1));
        vertices.Add(new Vector3(x + 1, y, z + 1));
        vertices.Add(new Vector3(x + 1, y, z));
        vertices.Add(new Vector3(x, y, z));
    }

    void CubeNorth(List<Vector3> vertices)
    {
        vertices.Add(new Vector3(x + 1, y - 1, z + 1));
        vertices.Add(new Vector3(x + 1, y, z + 1));
        vertices.Add(new Vector3(x, y, z + 1));
        vertices.Add(new Vector3(x, y - 1, z + 1));
    }
    void CubeEast(List<Vector3> vertices)
    {
        vertices.Add(new Vector3(x + 1, y - 1, z));
        vertices.Add(new Vector3(x + 1, y, z));
        vertices.Add(new Vector3(x + 1, y, z + 1));
        vertices.Add(new Vector3(x + 1, y - 1, z + 1));
    }
    void CubeSouth(List<Vector3> vertices)
    {
        vertices.Add(new Vector3(x, y - 1, z));
        vertices.Add(new Vector3(x, y, z));
        vertices.Add(new Vector3(x + 1, y, z));
        vertices.Add(new Vector3(x + 1, y - 1, z));
    }
    void CubeWest(List<Vector3> vertices)
    {
        vertices.Add(new Vector3(x, y - 1, z + 1));
        vertices.Add(new Vector3(x, y, z + 1));
        vertices.Add(new Vector3(x, y, z));
        vertices.Add(new Vector3(x, y - 1, z));
    }
    void CubeBot(List<Vector3> vertices)
    {
        vertices.Add(new Vector3(x, y - 1, z));
        vertices.Add(new Vector3(x + 1, y - 1, z));
        vertices.Add(new Vector3(x + 1, y - 1, z + 1));
        vertices.Add(new Vector3(x, y - 1, z + 1));
    }

    int Draw(List<int> triangles, List<Vector2> UV, int faceCount, bool top = false)
    {
        triangles.Add(faceCount * 4); //1
        triangles.Add(faceCount * 4 + 1); //2
        triangles.Add(faceCount * 4 + 2); //3
        triangles.Add(faceCount * 4); //1
        triangles.Add(faceCount * 4 + 2); //3
        triangles.Add(faceCount * 4 + 3); //4

        var texturePos = GetTexturePos(type, top);

        UV.Add(new Vector2(tUnit * texturePos.x + tUnit, tUnit * texturePos.y));
        UV.Add(new Vector2(tUnit * texturePos.x + tUnit, tUnit * texturePos.y + tUnit));
        UV.Add(new Vector2(tUnit * texturePos.x, tUnit * texturePos.y + tUnit));
        UV.Add(new Vector2(tUnit * texturePos.x, tUnit * texturePos.y));

        faceCount++;
        return faceCount;
    }
    public int DrawCube(List<Vector3> vertices, List<int> triangles, List<Vector2> UV, int faceCount)
    {
        if (chunk.Block(x, y + 1, z) == 0)
        {
            //Block above is air
            CubeTop(vertices);
            faceCount = Draw(triangles, UV, faceCount, true);
        }

        if (chunk.Block(x, y - 1, z) == 0)
        {
            //Block below is air
            CubeBot(vertices);
            faceCount = Draw(triangles, UV, faceCount);
        }

        if (chunk.Block(x + 1, y, z) == 0)
        {
            //Block east is air
            CubeEast(vertices);
            faceCount = Draw(triangles, UV, faceCount);
        }

        if (chunk.Block(x - 1, y, z) == 0)
        {
            //Block west is air
            CubeWest(vertices);
            faceCount = Draw(triangles, UV, faceCount);
        }

        if (chunk.Block(x, y, z + 1) == 0)
        {
            //Block north is air
            CubeNorth(vertices);
            faceCount = Draw(triangles, UV, faceCount);
        }

        if (chunk.Block(x, y, z - 1) == 0)
        {
            //Block south is air
            CubeSouth(vertices);
            faceCount = Draw(triangles, UV, faceCount);
        }

        return faceCount;
    }

    Vector2 GetTexturePos(int type, bool top)
    {
        if (type == 2)
        {
            if (!top)
                return tGrassTop;

            return tGrass;
        }
        return tStone;
    }
}
