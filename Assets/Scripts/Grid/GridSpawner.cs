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

    public Transform itemContainer;
    public GameObject minePrefab;
    public List<Vector2Int> mines;
    private List<Transform> mineTransforms = new List<Transform>();
    public GameObject heartPrefab;
    public List<Vector2Int> hearts;
    private List<Transform> heartTransforms = new List<Transform>();

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
        SpawnItems();
    }

    private void SpawnItems()
    {
        for (int i = mineTransforms.Count; i > 0; i--)
        {
            mineTransforms[i-1].parent = null;
            DestroyImmediate(mineTransforms[i-1].gameObject);
            mineTransforms.RemoveAt(i-1);
        }

        for (int i = heartTransforms.Count; i > 0; i--)
        {
            heartTransforms[i-1].parent = null;
            DestroyImmediate(heartTransforms[i-1].gameObject);
            heartTransforms.RemoveAt(i-1);
        }

        foreach (Vector2Int loc in mines)
        {
            Vector3 pos = grid.GetWorldPosition(loc.x, loc.y);
            pos.z = itemContainer.position.z;
            mineTransforms.Add(Instantiate(minePrefab, pos, Quaternion.identity,
                itemContainer).transform);
        }
        
        foreach (Vector2Int loc in hearts)
        {
            Vector3 pos = grid.GetWorldPosition(loc.x, loc.y);
            pos.z = itemContainer.position.z;
            heartTransforms.Add(Instantiate(heartPrefab, pos, Quaternion.identity,
                itemContainer).transform);
        }
    }

    public void EraseItems()
    {
        for (int i = mineTransforms.Count; i > 0; i--)
        {
            mineTransforms[i-1].parent = null;
            DestroyImmediate(mineTransforms[i-1].gameObject);
            mineTransforms.RemoveAt(i-1);
        }

        for (int i = heartTransforms.Count; i > 0; i--)
        {
            heartTransforms[i-1].parent = null;
            DestroyImmediate(heartTransforms[i-1].gameObject);
            heartTransforms.RemoveAt(i-1);
        }
        
        mines.Clear();
        hearts.Clear();
    }

    
}
