using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GridSpawner : MonoBehaviour
{
    public Heatmap heatmap;
    public Tilemap tilemap;
    public Fogmap fogmap;
    public Vector2Int gridSize;
    public float cellSize;
    public List<Vector2Int> goals;
    public GameObject goalPrefab;

    public Transform itemContainer;

    public GameObject minePrefab;
    public List<Vector2Int> mines;
    // private List<Transform> mineTransforms = new List<Transform>();
    public GameObject heartPrefab;
    public List<Vector2Int> hearts;
    // private List<Transform> heartTransforms = new List<Transform>();

    public GameObject playerPrefab;
    public Vector2Int playerStartPos;
    
    public bool autoUpdate;
    private Grid grid;
    void Start()
    {
        GenerateGrid();
        fogmap.GenerateFogmap(grid);
        Vector3 pos = grid.GetWorldPosition(playerStartPos.x, playerStartPos.y);
        pos.z = -1f;
        GameObject player = Instantiate(playerPrefab, pos, Quaternion.identity);
        player.GetComponent<PlayerController>().SetGridAndPlayer(grid, playerStartPos, fogmap);

        InstantiateGoals();
    }

    public void GenerateGrid()
    {
        grid = new Grid(gridSize.x, gridSize.y, cellSize, goals);
        heatmap.GenerateHeatmap(grid);
        tilemap.GenerateTilemap(grid);
        // SpawnItems();
    }

    private void InstantiateGoals()
    {
        foreach (Vector2Int goal in goals)
        {
            Vector3 pos = grid.GetWorldPosition(goal.x, goal.y);
            pos.z = itemContainer.position.z;
            Instantiate(goalPrefab, pos, Quaternion.identity, itemContainer);
        }
    }
    private void OnValidate()
    {
        if (playerStartPos.x < 0)
        {
            playerStartPos.x = 0;
        }
        if (playerStartPos.y < 0)
        {
            playerStartPos.y = 0;
        }
        if (playerStartPos.x >= gridSize.x)
        {
            playerStartPos.x = gridSize.x-1;
        }
        if (playerStartPos.y >= gridSize.y)
        {
            playerStartPos.y = gridSize.y-1;
        }
    }

    // private void SpawnItems()
    // {
    //     for (int i = mineTransforms.Count; i > 0; i--)
    //     {
    //         mineTransforms[i-1].parent = null;
    //         DestroyImmediate(mineTransforms[i-1].gameObject);
    //         mineTransforms.RemoveAt(i-1);
    //     }
    //
    //     for (int i = heartTransforms.Count; i > 0; i--)
    //     {
    //         heartTransforms[i-1].parent = null;
    //         DestroyImmediate(heartTransforms[i-1].gameObject);
    //         heartTransforms.RemoveAt(i-1);
    //     }
    //
    //     foreach (Vector2Int loc in mines)
    //     {
    //         Vector3 pos = grid.GetWorldPosition(loc.x, loc.y);
    //         pos.z = itemContainer.position.z;
    //         mineTransforms.Add(Instantiate(minePrefab, pos, Quaternion.identity,
    //             itemContainer).transform);
    //     }
    //     
    //     foreach (Vector2Int loc in hearts)
    //     {
    //         Vector3 pos = grid.GetWorldPosition(loc.x, loc.y);
    //         pos.z = itemContainer.position.z;
    //         heartTransforms.Add(Instantiate(heartPrefab, pos, Quaternion.identity,
    //             itemContainer).transform);
    //     }
    // }
    //
    // public void EraseItems()
    // {
    //     for (int i = mineTransforms.Count; i > 0; i--)
    //     {
    //         mineTransforms[i-1].parent = null;
    //         DestroyImmediate(mineTransforms[i-1].gameObject);
    //         mineTransforms.RemoveAt(i-1);
    //     }
    //
    //     for (int i = heartTransforms.Count; i > 0; i--)
    //     {
    //         heartTransforms[i-1].parent = null;
    //         DestroyImmediate(heartTransforms[i-1].gameObject);
    //         heartTransforms.RemoveAt(i-1);
    //     }
    //     
    //     mines.Clear();
    //     hearts.Clear();
    // }

    
}
