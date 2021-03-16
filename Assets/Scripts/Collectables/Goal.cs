using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public GameEvent onTreasureCollect;
    public SpriteRenderer sr;
    private float timer;
    private float framerate = .5f;
    public Sprite[] animations;
    private int currentFrame;
    private void OnTriggerEnter2D(Collider2D other)
    {
        Collect();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 1f)
        {
            timer -= 1f;
            currentFrame = currentFrame == 1 ? 0 : 1;
            sr.sprite = animations[currentFrame];
        }
    }

    private void Collect()
    {
        onTreasureCollect.Raise();
    }
}
