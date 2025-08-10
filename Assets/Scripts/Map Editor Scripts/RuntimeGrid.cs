using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class RuntimeGrid : MonoBehaviour
{
    public float cellSize = 1f;
    public int sizeX;
    public int sizeY;
    public Material lineMaterial;

    void Start()
    {
        DrawGrid(50, 50);
    }

    public void DrawGrid(int width, int height)
    {
        sizeX = width;
        sizeY = height;
        
        ClearGrid();

        Vector3 offset = new Vector3(-0.8f, -0.8f, 0f);

        for (int x = 0; x <= width; x++)
        {
            CreateLine(
                new Vector3(x * cellSize, 0, 0) + offset,
                new Vector3(x * cellSize, height * cellSize, 0) + offset
            );
        }

        for (int y = 0; y <= height; y++)
        {
            CreateLine(
                new Vector3(0, y * cellSize, 0) + offset,
                new Vector3(width * cellSize, y * cellSize, 0) + offset
            );
        }
    }
    public void ClearGrid()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                DestroyImmediate(transform.GetChild(i).gameObject);
            else
#endif
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    void CreateLine(Vector3 start, Vector3 end)
    {
        GameObject lineObj = new GameObject("GridLine");
        lineObj.transform.parent = transform;
        LineRenderer lr = lineObj.AddComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.SetPositions(new Vector3[] { start, end });
        lr.material = lineMaterial;
        lr.startWidth = lr.endWidth = 0.05f;
        lr.useWorldSpace = true;
        lr.sortingOrder = 8;
    }
}
