using System;
using UnityEngine;

public class HighlightGrid : MonoBehaviour
{
    public GameObject highlight;
    private RuntimeGrid grid;

    private void Awake()
    {
        grid = GetComponent<RuntimeGrid>();
    }

    private void Update()
    {
        float cellSize = grid.cellSize; // Should be 1.6f
        int columns = grid.width;
        int rows = grid.height;
        Vector3 offset = new Vector3(-0.8f, -0.8f, 0f);

        // Convert mouse position to world, then to grid-local coordinates
        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition) - offset;

        // Calculate grid indices
        int gridX = Mathf.FloorToInt(mouseWorld.x / cellSize);
        int gridY = Mathf.FloorToInt(mouseWorld.y / cellSize);

        // Clamp to grid bounds
        gridX = Mathf.Clamp(gridX, 0, columns - 1);
        gridY = Mathf.Clamp(gridY, 0, rows - 1);

        // Calculate the world position of the cell center
        Vector2 cellWorldPos = new Vector2(
            gridX * cellSize + cellSize / 2f,
            gridY * cellSize + cellSize / 2f
        ) + new Vector2(-0.8f, -0.8f);

        HighlightSquare(cellWorldPos);
    }

    private void HighlightSquare(Vector2 pos)
    {
        highlight.transform.position = pos;
    }
}