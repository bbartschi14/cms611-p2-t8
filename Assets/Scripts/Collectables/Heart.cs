using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    public GameEvent onHeartCollect;
    public SpriteRenderer sr;
    private float timer;
    private float framerate = .5f;
    public Sprite[] animations;
    private int currentFrame;
    private bool activated = false;
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
        if (!activated)
        {
            onHeartCollect.Raise();
            LeanTween.value(gameObject, color => sr.color = color, sr.color, 
                new Color(1f, 1f, 1f, .5f), .5f).setDelay(.5f);
            activated = true;
        }
    }
}
