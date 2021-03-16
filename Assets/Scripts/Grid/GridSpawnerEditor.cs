using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (GridSpawner))]
public class GridSpawnerEditor : Editor
{
    public override void OnInspectorGUI() {
        GridSpawner gridSpawner = (GridSpawner)target;

        if (DrawDefaultInspector ()) {
            if (gridSpawner.autoUpdate)
            {
                gridSpawner.GenerateGrid();
            }
        }
        
        
        if (GUILayout.Button("Generate")) {
            gridSpawner.GenerateGrid();
        }
        
        // if (GUILayout.Button("Erase Items")) {
        //     gridSpawner.EraseItems();
        // }

    }
}