using System.Collections.Generic;
using UnityEngine;

public static class HexUtility
{
    

    #region Coord Pixel Convertions
    //The distance between any two neighbours
    public static float DistanceBetweenNeighbours(float size = 1f)
    {
        var x = size * (3f / 2);
        var y = size * (Mathf.Sqrt(3f) / 2f * + Mathf.Sqrt(3f));

        return Mathf.Sqrt(x * x + y * y);
    }

    public static Vector2 PixelFromCubeCoord(Vector3Int h, float size = 1f)
    {
        var x = size * (3f / 2 * h.x);
        var y = size * (Mathf.Sqrt(3f) / 2f * h.x + Mathf.Sqrt(3f) * h.y);

        return new Vector2(x, y);
    }
    #endregion

    #region Grid
    public static List<Vector3Int> CreateGrid(int dist)
    {
        var grid = new List<Vector3Int>();

        for (int x = -dist; x <= dist; x++)
        {
            for (int z = -dist; z <= dist; z++)
            {
                Vector3Int cubeCoord = CubeCoord(x, z);

                if (CubeCoordDistance(Vector3Int.zero, cubeCoord) <= dist)
                {
                    grid.Add(cubeCoord);
                }
            }
        }

        return grid;
    }

    private static readonly int[] spiralDirs = new int[] { 4, 3, 2, 1, 0, 5 };

    public static List<Vector3Int> Spiral(int radius)
    {
        List<Vector3Int> spiral = new List<Vector3Int>()
        {
            Vector3Int.zero
        };

        for (int k = 1; k <= radius; k++)
        {
            var curr = Neighbour(Vector3Int.zero, 0, k);

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    spiral.Add(curr);
                    curr = Neighbour(curr, spiralDirs[i]);
                }
            }
        }

        return spiral;
    }

    #endregion

    #region Neighbours
    public static Vector3Int[] NeighbourDirections = new Vector3Int[]
    {
        new Vector3Int( 1, -1,  0),
        new Vector3Int( 1,  0, -1),
        new Vector3Int( 0,  1, -1),
        new Vector3Int(-1,  1,  0),
        new Vector3Int(-1,  0,  1),
        new Vector3Int( 0, -1,  1),
    };


    public static Vector3Int Neighbour(Vector3Int hexPos, int dir, int dist = 1)
    {

        return new Vector3Int(hexPos.x + NeighbourDirections[dir].x * dist, hexPos.y + NeighbourDirections[dir].y * dist, hexPos.z + NeighbourDirections[dir].z * dist);
    }
    #endregion

    #region Coords
    public static Vector2Int AxialFromCube(Vector3Int cube)
    {
        return new Vector2Int(cube.x, cube.z);
    }

    public static Vector3Int CubeCoord(int x, int z)
    {
        return new Vector3Int(x, -x - z, z);
    }

    public static int CubeCoordDistance(Vector3Int a, Vector3Int b)
    {
        return Mathf.Max(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y), Mathf.Abs(a.z - b.z));
    }
    #endregion
}