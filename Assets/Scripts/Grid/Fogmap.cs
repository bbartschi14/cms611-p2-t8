using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Fogmap : MonoBehaviour
{
    private Grid grid;
    private Texture2D texture;

    private GameObject[,] fogGrid;
    public GameObject fogPrefab;

    public void GenerateFogmap(Grid grid)
    {
        this.grid = grid;
        fogGrid = new GameObject[grid.GetWidth(), grid.GetHeight()];
        CreateFog();
    }

    public void DeactivateSquare(Vector2Int loc)
    {
        if (loc.x >= 0 && loc.y >= 0 && loc.x < grid.GetWidth() && loc.y < grid.GetHeight())
        {
            fogGrid[loc.x, loc.y].GetComponent<Fog>().DeactiveSelf();
        }
    }

    private void CreateFog()
    {
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y< grid.GetHeight(); y++)
            {
                Vector3 pos = grid.GetWorldPosition(x, y);
                pos.z = transform.position.z;
                fogGrid[x, y] = Instantiate(fogPrefab, pos, Quaternion.identity,
                    transform);
            }
        }
    }


    
}
