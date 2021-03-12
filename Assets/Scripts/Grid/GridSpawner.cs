using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    public Heatmap heatmap;
    public Tilemap tilemap;
    public Vector2Int gridSize;
    public float cellSize;
    public List<Vector2Int> goals;
    public bool autoUpdate;
    private Grid grid;
    void Start()
    {
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        grid = new Grid(gridSize.x, gridSize.y, cellSize, goals);
        heatmap.GenerateHeatmap(grid);
        tilemap.GenerateTilemap(grid);
    }

    
}
