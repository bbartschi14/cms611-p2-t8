using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{
    public SpriteRenderer sr;
    private bool active = true;
    public void DeactiveSelf()
    {
        if (active)
        {
            LeanTween.value(gameObject, color => sr.color = color, sr.color, Color.clear, .25f);
            active = false;
        }
        
    }
}
