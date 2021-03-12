using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Grid
{
    private int width;
    private int height;
    private float cellSize;

    private List<Vector2Int> goals;
    public Grid(int width, int height, float cellSize, List<Vector2Int> goals)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.goals = goals;
    }

    public int GetWidth()
    {
        return width;
    }
    public int GetHeight()
    {
        return height;
    }
    
    public float GetCellSize()
    {
        return cellSize;
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize;
    }

    public Vector2Int GetXYFromWorldPosition(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt(worldPosition.x / cellSize);
        int y = Mathf.FloorToInt(worldPosition.y / cellSize);
        return new Vector2Int(x, y);
    }

    public float GetGoalProximity(int x, int y, int maxDistance)
    {
        if (x < 0 || y < 0 || y >= height || x >= width) return 0f;

        float value = 0f;
        
        foreach (Vector2Int goal in goals)
        {
            int dist = IntDistance(goal, new Vector2Int(x, y));
            if (dist > maxDistance) continue;

            value += (float) (maxDistance - dist) / maxDistance;
        }
        //Debug.Log("Distance to " + x + " " + y + " = " + Mathf.Min(value, 1f));
        return Mathf.Min(value, 1f);
    }

    private float DistanceBetweenPoints(Vector2Int p0, Vector2Int p1)
    {
        return Mathf.Sqrt(Mathf.Pow(p1.x - p0.x,2) + Mathf.Pow(p1.y - p0.y,2));
    }

    private int IntDistance(Vector2Int p0, Vector2Int p1)
    {
        return Mathf.Abs(p1.x - p0.x) + Mathf.Abs(p1.y - p0.y);
    }
    
}
