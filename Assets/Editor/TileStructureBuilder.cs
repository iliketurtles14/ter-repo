using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class TileStructureBuilder : EditorWindow
{
    private static readonly float tileSize = 1.6f;
    private static readonly string[] tags = { "Wall", "Bars", "Obstacle", "Fence", "ElectricFence" };

    [MenuItem("Tools/Generate Polygon Collider from Tiles")]
    public static void Generate()
    {
        Dictionary<Vector2Int, bool> tileMap = new();

        foreach (var obj in GameObject.FindObjectsOfType<GameObject>())
        {
            foreach (var tag in tags)
            {
                if (obj.CompareTag(tag))
                {
                    Vector2Int gridPos = WorldToGrid(obj.transform.position);
                    tileMap[gridPos] = true;
                    break;
                }
            }
        }

        if (tileMap.Count == 0)
        {
            Debug.LogWarning("No tiles found with matching tags.");
            return;
        }

        // Determine bounds
        Vector2Int min = new Vector2Int(int.MaxValue, int.MaxValue);
        Vector2Int max = new Vector2Int(int.MinValue, int.MinValue);

        foreach (var key in tileMap.Keys)
        {
            min = Vector2Int.Min(min, key);
            max = Vector2Int.Max(max, key);
        }

        int width = max.x - min.x + 3;
        int height = max.y - min.y + 3;
        bool[,] grid = new bool[width, height];

        // Fill grid with tile presence
        foreach (var key in tileMap.Keys)
        {
            int x = key.x - min.x + 1;
            int y = key.y - min.y + 1;
            grid[x, y] = true;
        }

        // Run marching squares
        List<Vector2> outline = MarchingSquares(grid, min);

        if (outline.Count < 3)
        {
            Debug.LogWarning("Not enough points to create a polygon.");
            return;
        }

        GameObject colliderObject = new GameObject("GeneratedCollider");
        PolygonCollider2D poly = colliderObject.AddComponent<PolygonCollider2D>();
        poly.SetPath(0, outline.ToArray());

        Debug.Log("Polygon collider generated successfully.");
    }

    static Vector2Int WorldToGrid(Vector3 pos)
    {
        return new Vector2Int(
            Mathf.RoundToInt(pos.x / tileSize),
            Mathf.RoundToInt(pos.y / tileSize)
        );
    }

    static Vector2 GridToWorld(Vector2Int grid, Vector2Int offset)
    {
        return new Vector2(
            (grid.x + offset.x - 1) * tileSize,
            (grid.y + offset.y - 1) * tileSize
        );
    }

    static List<Vector2> MarchingSquares(bool[,] grid, Vector2Int offset)
    {
        List<Vector2> path = new();
        int width = grid.GetLength(0);
        int height = grid.GetLength(1);

        // Directions for tracing
        Vector2Int[] dirs = {
            new Vector2Int(0, 1),  // up
            new Vector2Int(1, 0),  // right
            new Vector2Int(0, -1), // down
            new Vector2Int(-1, 0)  // left
        };

        // Find starting point
        for (int y = 0; y < height - 1; y++)
        {
            for (int x = 0; x < width - 1; x++)
            {
                if (grid[x, y])
                {
                    Vector2Int current = new Vector2Int(x, y);
                    Vector2Int start = current;
                    Vector2Int dir = dirs[0];
                    bool looped = false;

                    do
                    {
                        int state =
                            (grid[current.x, current.y] ? 1 : 0) +
                            (grid[current.x + 1, current.y] ? 2 : 0) +
                            (grid[current.x + 1, current.y + 1] ? 4 : 0) +
                            (grid[current.x, current.y + 1] ? 8 : 0);

                        Vector2 point = MarchingSquaresInterp(current, state, offset);
                        if (point != Vector2.zero)
                            path.Add(point);

                        // Turn based on state
                        switch (state)
                        {
                            case 1: case 5: case 13: dir = dirs[3]; break;
                            case 8: case 10: case 11: dir = dirs[0]; break;
                            case 4: case 12: case 14: dir = dirs[1]; break;
                            case 2: case 3: case 7: dir = dirs[2]; break;
                            default: dir = dirs[0]; break;
                        }

                        current += dir;

                        if (current == start && path.Count > 2)
                            looped = true;

                    } while (!looped && path.Count < 10000);

                    return path;
                }
            }
        }

        return path;
    }

    static Vector2 MarchingSquaresInterp(Vector2Int cell, int state, Vector2Int offset)
    {
        float x = (cell.x + offset.x - 1) * tileSize;
        float y = (cell.y + offset.y - 1) * tileSize;

        switch (state)
        {
            case 1: return new Vector2(x, y + tileSize * 0.5f);
            case 2: return new Vector2(x + tileSize * 0.5f, y);
            case 3: return new Vector2(x + tileSize * 0.5f, y + tileSize * 0.5f);
            case 4: return new Vector2(x + tileSize, y + tileSize * 0.5f);
            case 5: return new Vector2(x + tileSize * 0.5f, y + tileSize);
            case 6: return new Vector2(x + tileSize * 0.5f, y);
            case 7: return new Vector2(x + tileSize * 0.5f, y);
            case 8: return new Vector2(x, y + tileSize * 0.5f);
            case 9: return new Vector2(x, y + tileSize * 0.5f);
            case 10: return new Vector2(x + tileSize * 0.5f, y + tileSize);
            case 11: return new Vector2(x, y + tileSize * 0.5f);
            case 12: return new Vector2(x + tileSize * 0.5f, y + tileSize);
            case 13: return new Vector2(x + tileSize * 0.5f, y + tileSize);
            case 14: return new Vector2(x + tileSize, y + tileSize * 0.5f);
            default: return Vector2.zero;
        }
    }
}
