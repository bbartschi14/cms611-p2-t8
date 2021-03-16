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
    public GameObject tentaclePrefab;
    public Sprite[] tentacleSprites;
    public List<Vector2Int> mines;

    // private List<Transform> mineTransforms = new List<Transform>();
    public GameObject heartPrefab;
    public List<Vector2Int> hearts;
    // private List<Transform> heartTransforms = new List<Transform>();

    public GameObject playerPrefab;
    public Vector2Int playerStartPos;
    
    public bool autoUpdate;
    private Grid grid;
    private int[,] adjacencyMatrix;
    void Start()
    {
        GenerateGrid();
        fogmap.GenerateFogmap(grid);
        Vector3 pos = grid.GetWorldPosition(playerStartPos.x, playerStartPos.y);
        pos.z = -1f;
        GameObject player = Instantiate(playerPrefab, pos, Quaternion.identity);
        player.GetComponent<PlayerController>().SetGridAndPlayer(grid, playerStartPos, fogmap);

        InstantiateGoals();
        InstantiateMines();
        InstantiateHearts();

        FillAdjacency();
        SpawnAllTentacles();
    }

    public void GenerateGrid()
    {
        grid = new Grid(gridSize.x, gridSize.y, cellSize, goals);
        heatmap.GenerateHeatmap(grid);
        tilemap.GenerateTilemap(grid);
        // SpawnItems();
    }

    public Grid GetGrid()
    {
        return grid;
    }

    private void SpawnAllTentacles()
    {
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                SpawnGridTentacles(x, y, adjacencyMatrix[x,y]);
            }
        }
    }

    private void SpawnGridTentacles(int x, int y, int num)
    {
        int[] positions = new int[] {0, 1, 2, 3, 4, 5, 6, 7};
        
        for (int t = 0; t < positions.Length; t++ )
        {
            int tmp = positions[t];
            int r = UnityEngine.Random.Range(t, positions.Length);
            positions[t] = positions[r];
            positions[r] = tmp;
        }
        for (int i = 0; i < num; i++)
        {
            Vector3 pos = grid.GetWorldPosition(x, y);
            pos.z = itemContainer.position.z;
            GameObject tentacle = Instantiate(tentaclePrefab, pos, Quaternion.identity, itemContainer);
            tentacle.GetComponentInChildren<SpriteRenderer>().sprite = tentacleSprites[positions[i]];
        }
    }
    
    

    private void FillAdjacency()
    {
        adjacencyMatrix = new int[grid.GetWidth(), grid.GetHeight()];
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                int value = 0;
                foreach (Vector2Int mine in mines)
                {
                    value += IsAdjacent(x, y, mine.x, mine.y) ? 1 : 0;
                }
                //Debug.Log("X, Y: " + x + ", " + y + " is " + value);
                adjacencyMatrix[x, y] = value;
            }
        }
    }

    private bool IsAdjacent(int x1, int y1, int x2, int y2)
    {
        if (x2 >= x1 - 1 && x2 <= x1 + 1)
        {
            if (y2 >= y1 - 1 && y2 <= y1 + 1)
            {
                if (!(x1 == x2 && y1 == y2)) return true;
            }
        }

        return false;
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
    
    private void InstantiateMines()
    {
        foreach (Vector2Int mine in mines)
        {
            Vector3 pos = grid.GetWorldPosition(mine.x, mine.y);
            pos.z = itemContainer.position.z;
            Instantiate(minePrefab, pos, Quaternion.identity, itemContainer);
        }
    }
    
    private void InstantiateHearts()
    {
        foreach (Vector2Int heart in hearts)
        {
            Vector3 pos = grid.GetWorldPosition(heart.x, heart.y);
            pos.z = itemContainer.position.z;
            Instantiate(heartPrefab, pos, Quaternion.identity, itemContainer);
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
