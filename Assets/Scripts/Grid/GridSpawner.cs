using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

public class GridSpawner : MonoBehaviour
{
    
    public Heatmap heatmap;
    public Tilemap tilemap;
    public Fogmap fogmap;
    public Vector2Int gridSize;
    public float cellSize;
    public List<Vector2Int> goals;
    private int treasuresCollected;
    private int totalTreasures;
    public GameObject goalPrefab;
    public GameEvent onGameWon;
    public Transform itemContainer;

    public GameObject minePrefab;
    public GameObject tentaclePrefab;
    
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

    private List<GameObject>[,] tentacles;

    public bool randomMap;
    public int numGoals;
    public int numOcto;
    public int numLife;
    private List<Vector2Int> generatedGoals;
    private List<Vector2Int> generatedMines;
    private List<Vector2Int> generatedHearts;
    private Vector2Int generatedPlayerPos;
    void Start()
    {
        if (randomMap)
        {
            GenerateRandom();
            RandomizeMap();
        }
        else
        {
            GenerateGrid();
            fogmap.GenerateFogmap(grid);
            Vector3 pos = grid.GetWorldPosition(playerStartPos.x, playerStartPos.y);
            pos.z = -.2f;
            GameObject player = Instantiate(playerPrefab, pos, Quaternion.identity);
            player.GetComponent<PlayerController>().SetGridAndPlayer(grid, playerStartPos, fogmap);

            InstantiateGoals(goals);
            InstantiateMines(mines);
            InstantiateHearts(hearts);

            FillAdjacency(mines);
            SpawnAllTentacles();
            totalTreasures = goals.Count;
        }
        
    }

    public List<Vector2Int> GetGenGoals()
    {
        return generatedGoals;
    }
    
    public List<Vector2Int> GetGenMines()
    {
        return generatedMines;
    }
    public List<Vector2Int> GetGenHearts()
    {
        return generatedHearts;
    }
    public Vector2Int GetGenPos()
    {
        return generatedPlayerPos;
    }

    public void GenerateRandom()
    {
        generatedGoals = new List<Vector2Int>();
        generatedMines = new List<Vector2Int>();
        generatedHearts = new List<Vector2Int>();
        generatedPlayerPos = new Vector2Int();

        List<Vector2Int> possiblePositions = new List<Vector2Int>();
        for (int i = 0; i < gridSize.x; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {
                possiblePositions.Add(new Vector2Int(i,j));
            }
        }

        for (int i = 0; i < numGoals; i++)
        {
            int r;
            int count = 0;
            do
            {
                r = UnityEngine.Random.Range(0, possiblePositions.Count);
                count++;
                //Debug.Log(count);
            } while (!NoGoalsAdjacent(possiblePositions[r]) && count < 10);
            generatedGoals.Add(possiblePositions[r]);
            possiblePositions.RemoveAt(r);
        }
        
        for (int i = 0; i < numOcto; i++)
        {
            int r;
            int count = 0;
            do
            {
                r = UnityEngine.Random.Range(0, possiblePositions.Count);
                count++;
                //Debug.Log(count);
            } while (!NoMinesAdjacent(possiblePositions[r]) && count < 10);
            
            generatedMines.Add(possiblePositions[r]);
            possiblePositions.RemoveAt(r);
        }
        
        for (int i = 0; i < numLife; i++)
        {
            int r = UnityEngine.Random.Range(0, possiblePositions.Count);
            generatedHearts.Add(possiblePositions[r]);
            possiblePositions.RemoveAt(r);
        }

        generatedPlayerPos = possiblePositions[UnityEngine.Random.Range(0, possiblePositions.Count)];
        
        GenerateGrid();
        
    }

    private bool NoMinesAdjacent(Vector2Int pos)
    {
        foreach (Vector2Int mine in generatedMines)
        {
            if (IsAdjacent(pos.x, pos.y, mine.x, mine.y))
            {
                return false;
            }
        }

        return true;
    }

    private bool NoGoalsAdjacent(Vector2Int pos)
    {
        int rad = 3;
        foreach (Vector2Int goal in generatedGoals)
        {
            if (pos.x > goal.x - 3 && pos.x < goal.x + 3)
            {
                if (pos.y > goal.y - 3 && pos.y < goal.y + 3)
                {
                    return false;
                }
            }
        }

        return true;
    }


    private void RandomizeMap()
    {
        fogmap.GenerateFogmap(grid);
        Vector3 pos = grid.GetWorldPosition(generatedPlayerPos.x, generatedPlayerPos.y);
        pos.z = -.2f;
        GameObject player = Instantiate(playerPrefab, pos, Quaternion.identity);
        player.GetComponent<PlayerController>().SetGridAndPlayer(grid, generatedPlayerPos, fogmap);

        InstantiateGoals(generatedGoals);
        InstantiateMines(generatedMines);
        InstantiateHearts(generatedHearts);

        FillAdjacency(generatedMines);
        SpawnAllTentacles();
        totalTreasures = generatedGoals.Count;
    }

    public void GenerateGrid()
    {
        if (randomMap)
        {
            grid = new Grid(gridSize.x, gridSize.y, cellSize, generatedGoals);
        }
        else
        {
            grid = new Grid(gridSize.x, gridSize.y, cellSize, goals);
        }
        
        heatmap.GenerateHeatmap(grid);
        tilemap.GenerateTilemap(grid);
        // SpawnItems();
    }

    public void TreasureCollect()
    {
        treasuresCollected++;
        Debug.Log(treasuresCollected);
        if (treasuresCollected == totalTreasures)
        {
            onGameWon.Raise();
        }
    }
    public void DeleteGoal(Vector2Int goal)
    {
        if (randomMap)
        {
            generatedGoals.Remove(goal);
        }
        else
        {
            goals.Remove(goal);
        }
        
        heatmap.GenerateHeatmap(grid);
    }

    public Grid GetGrid()
    {
        return grid;
    }

    public void RemoveAdjacentTentacles(Vector2Int pos)
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i != 0 || j != 0)
                {
                    RemoveTentacleAt(pos.x + i, pos.y + j);
                }
            }
        }
    }

    private void RemoveTentacleAt(int x, int y)
    {
        if (x < 0 || y < 0 || y >= grid.GetHeight() || x >= grid.GetWidth()) return;
        
        List<GameObject> currTentacles = tentacles[x, y];
        int size = currTentacles.Count;
        if (size > 0)
        {
            currTentacles[size - 1].GetComponent<SingleTentacle>().RemoveSelf();
            currTentacles.RemoveAt(size-1);
        }
    }

    private void SpawnAllTentacles()
    {
        tentacles = new List<GameObject>[grid.GetWidth(), grid.GetHeight()];
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                tentacles[x, y] = new List<GameObject>();
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
            SingleTentacle ts = tentacle.GetComponent<SingleTentacle>();
            ts.SetID(positions[i]);
            tentacles[x,y].Add(tentacle);
        }
    }
    
    

    private void FillAdjacency(List<Vector2Int> minesList)
    {
        adjacencyMatrix = new int[grid.GetWidth(), grid.GetHeight()];
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                int value = 0;
                foreach (Vector2Int mine in minesList)
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

    private void InstantiateGoals(List<Vector2Int> goalsList)
    {
        foreach (Vector2Int goal in goalsList)
        {
            Vector3 pos = grid.GetWorldPosition(goal.x, goal.y);
            pos.z = itemContainer.position.z;
            GameObject g = Instantiate(goalPrefab, pos, Quaternion.identity, itemContainer);
            g.GetComponent<Goal>().SetPosition(goal);
        }
    }
    
    private void InstantiateMines(List<Vector2Int> minesList)
    {
        foreach (Vector2Int mine in minesList)
        {
            Vector3 pos = grid.GetWorldPosition(mine.x, mine.y);
            pos.z = itemContainer.position.z;
            GameObject octo = Instantiate(minePrefab, pos, Quaternion.identity, itemContainer);
            octo.GetComponent<Mine>().SetPos(mine);
        }
    }
    
    private void InstantiateHearts(List<Vector2Int> heartsList)
    {
        foreach (Vector2Int heart in heartsList)
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

}
