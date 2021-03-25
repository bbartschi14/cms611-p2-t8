using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public AudioSource treasureSFX;
    public GameEvent onTreasureCollect;
    public SpriteRenderer sr;
    private float timer;
    private float framerate = .5f;
    public Sprite[] animations;
    private int currentFrame;

    private bool collected = false;
    private Vector2Int pos;
    public List<Sprite> collectSprites = new List<Sprite>();
    public float collectTime = 1f;
    
    public GameEvent pausePlayer;
    public GameEvent resumePlayer;

    public Vector2IntGameEvent onRemoveGoal;

    public void SetPosition(Vector2Int position)
    {
        pos = position;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Collect();
    }

    private void Update()
    {
        Animate();
        
    }

    private void Animate()
    {
        if (!collected)
        {
            timer += Time.deltaTime;
            if (timer > 1f)
            {
                timer -= 1f;
                currentFrame = currentFrame == 1 ? 0 : 1;
                sr.sprite = animations[currentFrame];
            }
        }
    }

    private void Collect()
    {
        if (!collected)
        {
            treasureSFX.Play();
            pausePlayer.Raise();
            transform.position += new Vector3(0f, 0f, -1f);
            onTreasureCollect.Raise();
            StartCoroutine(StartAnimation(collectSprites, collectTime));
            onRemoveGoal.Raise(pos);
            collected = true;
        }
    }
    
    IEnumerator StartAnimation(List<Sprite> sprites, float time)
    {
        yield return new WaitForSeconds(.25f);
        LeanTween.value(gameObject, color => sr.color = color, sr.color, 
                new Color(1f, 1f, 1f, 0f), collectTime).setDelay(collectTime*.5f)
            .setOnComplete(() => resumePlayer.Raise());
        foreach (Sprite frame in sprites)
        {
            sr.sprite = frame;
            yield return new WaitForSeconds(time / sprites.Count);
        }
        yield break;
    }
}
