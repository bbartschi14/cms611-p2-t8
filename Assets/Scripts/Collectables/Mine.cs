using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public GameEvent onDamageTaken;
    private bool activated = false;
    public SpriteRenderer sr;
    private void OnTriggerEnter2D(Collider2D other)
    {
        Damage();
    }
    
    private void Damage()
    {
        if (!activated)
        {
            onDamageTaken.Raise();
            LeanTween.value(gameObject, color => sr.color = color, sr.color, 
                new Color(1f, 1f, 1f, .5f), .5f).setDelay(.5f);
            activated = true;
        }
        
    }
}
