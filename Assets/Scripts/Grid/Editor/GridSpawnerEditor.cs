using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (GridSpawner))]
public class GridSpawnerEditor : Editor
{
    private GridSpawner gridSpawner;
    private Grid grid;
    public override void OnInspectorGUI() {
        gridSpawner = (GridSpawner)target;

        if (DrawDefaultInspector ()) {
            if (gridSpawner.autoUpdate)
            {
                gridSpawner.GenerateGrid();
                grid = gridSpawner.GetGrid();
            }
        }
        
        
        if (GUILayout.Button("Generate")) {
            gridSpawner.GenerateGrid();
            grid = gridSpawner.GetGrid();
        }

    }

    private void OnSceneGUI()
    {
        gridSpawner = (GridSpawner)target;
        if (gridSpawner.randomMap)
        {
            gridSpawner.GenerateRandom();
        }
        else
        {
            gridSpawner.GenerateGrid();
        }
        
        grid = gridSpawner.GetGrid();
        
        if (grid != null)
        {
            Handles.color = Color.red;
            foreach (Vector2Int loc in gridSpawner.randomMap ? gridSpawner.GetGenMines() : gridSpawner.mines)
            {
                Vector3 gridPos = grid.GetWorldPosition(loc.x, loc.y);
                gridPos.x += grid.GetCellSize() * .5f;
                gridPos.y += grid.GetCellSize() * .5f;
                Handles.DrawSolidDisc(gridPos,Vector3.back, grid.GetCellSize()*.25f);
            }
            
            Handles.color = Color.green;
            foreach (Vector2Int loc in gridSpawner.randomMap ? gridSpawner.GetGenHearts() :gridSpawner.hearts)
            {
                Vector3 gridPos = grid.GetWorldPosition(loc.x, loc.y);
                gridPos.x += grid.GetCellSize() * .5f;
                gridPos.y += grid.GetCellSize() * .5f;
                Handles.DrawSolidDisc(gridPos,Vector3.back, grid.GetCellSize()*.25f);
            }
            
            Handles.color = Color.blue;
            Vector2Int pos = gridSpawner.randomMap ? gridSpawner.GetGenPos() : gridSpawner.playerStartPos;
            Vector3 playerPos = grid.GetWorldPosition(pos.x, pos.y);
            playerPos.x += grid.GetCellSize() * .5f;
            playerPos.y += grid.GetCellSize() * .5f;
            Handles.DrawSolidDisc(playerPos,Vector3.back, grid.GetCellSize()*.25f);
        }
    }
}