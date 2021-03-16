using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureContainer : MonoBehaviour
{
    public GameObject singleTreasurePrefab;
    private List<SingleTreasure> treasures = new List<SingleTreasure>();
    public int totalTreasures;
    private int currentTreasures;
    
    private void Start()
    {
        currentTreasures = 0;
        for (int i = 0; i < totalTreasures; i++)
        {
            GameObject treasure = Instantiate(singleTreasurePrefab, transform);
            treasures.Add(treasure.GetComponent<SingleTreasure>());
            treasures[i].SetEmpty();
        }
    }

    public void GetTreasure()
    {
        currentTreasures++;
        treasures[currentTreasures-1].SetFull();
    }
}
