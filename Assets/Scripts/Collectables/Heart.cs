using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    public AudioSource heartSFX;
    public GameEvent onHeartCollect;
    public SpriteRenderer sr;
    private float timer;
    private float framerate = .5f;
    public Sprite[] animations;
    private int currentFrame;
    private bool activated = false;
    
    public List<Sprite> collectSprites = new List<Sprite>();
    public float collectTime = 1f;
    
    public GameEvent pausePlayer;
    public GameEvent resumePlayer;
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
        if (!activated)
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
        if (!activated)
        {
            heartSFX.Play();
            pausePlayer.Raise();
            transform.position += new Vector3(0f, 0f, -1f);
            StartCoroutine(StartAnimation(collectSprites, collectTime));
            onHeartCollect.Raise();
            LeanTween.value(gameObject, color => sr.color = color, sr.color, 
                new Color(1f, 1f, 1f, 0f), collectTime).setDelay(collectTime*.5f)
                .setOnComplete(() => resumePlayer.Raise());;
            activated = true;
        }
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
