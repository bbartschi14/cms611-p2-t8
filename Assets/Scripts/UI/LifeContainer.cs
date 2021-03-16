using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeContainer : MonoBehaviour
{
    public GameObject singleLifePrefab;
    public GameEvent onPlayerDied;
    public int startingLife;
    private List<SingleLife> lives = new List<SingleLife>();
    private int currentHealth;
    private void Start()
    {
        currentHealth = startingLife;
        for (int i = 0; i < startingLife; i++)
        {
            GameObject life = Instantiate(singleLifePrefab, transform);
            lives.Add(life.GetComponent<SingleLife>());
            lives[i].SetFull();
        }
    }

    public void GetLife()
    {
        currentHealth++;
        if (currentHealth > lives.Count)
        {
            GameObject life = Instantiate(singleLifePrefab, transform);
            lives.Add(life.GetComponent<SingleLife>());
        }
        lives[currentHealth-1].SetFull();
    }

    public void LoseLife()
    {
        if (currentHealth == 0) return;
        lives[currentHealth-1].SetEmpty();
        currentHealth--;
        
        if (currentHealth == 0)
        {
            onPlayerDied.Raise();
            return;
        }
    }
}
