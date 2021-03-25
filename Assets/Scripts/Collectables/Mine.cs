using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public AudioSource octoSFX;
    public GameEvent onDamageTaken;
    private bool activated = false;
    public SpriteRenderer sr;

    public List<Sprite> riseSprites = new List<Sprite>();
    public float riseTime = 1f;
    public List<Sprite> attackSprites = new List<Sprite>();
    public float attackTime = 1f;
    public int numAttacks = 1;
    public List<Sprite> descendSprites = new List<Sprite>();
    public float descendTime = 1f;

    public GameEvent pausePlayer;
    public GameEvent resumePlayer;
    public Vector2IntGameEvent onRemoveTentacles;

    private Vector2Int pos;

    public void SetPos(Vector2Int gridPos)
    {
        this.pos = gridPos;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Damage();
    }
    
    private void Damage()
    {
        if (!activated)
        {
            StartCoroutine(AnimateAll());
            activated = true;
        }
        
    }

    IEnumerator AnimateAll()
    {
        pausePlayer.Raise();
        onRemoveTentacles.Raise(pos);
        octoSFX.Play();
        yield return StartCoroutine(StartAnimation(riseSprites, riseTime));
        
        onDamageTaken.Raise();
        for (int i = 0; i < numAttacks; i++)
        {
            yield return StartCoroutine(StartAnimation(attackSprites, attackTime));
        }
        yield return StartCoroutine(StartAnimation(descendSprites, descendTime));
        sr.color = Color.clear;
        resumePlayer.Raise();
    }

    IEnumerator StartAnimation(List<Sprite> sprites, float time)
    {
        foreach (Sprite frame in sprites)
        {
            sr.sprite = frame;
            yield return new WaitForSeconds(time / sprites.Count);
        }
        yield break;
    }
}
