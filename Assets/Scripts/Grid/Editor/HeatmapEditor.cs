using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (Heatmap))]
public class HeatmapEditor : Editor
{
    public override void OnInspectorGUI() {
        Heatmap heatMap = (Heatmap)target;

        if (DrawDefaultInspector ()) {
            if (heatMap.autoUpdate && heatMap.grid != null) {
                heatMap.GenerateHeatmap(heatMap.grid);
            }
        }

        
    }
}