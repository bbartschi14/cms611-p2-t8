using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Fog : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public SpriteRenderer sr;
    public SpriteRenderer marker;
    private bool active = true;
    public bool markerActive = false;
    
    public void DeactiveSelf()
    {
        if (active)
        {
            LeanTween.value(gameObject, color => sr.color = color, sr.color, Color.clear, .7f);
            active = false;
            marker.color = new Color(1f, 1f, 1f, 0f);
            markerActive = false;
        }
        
    }

    private void ToggleMarker()
    {
        float newAlpha = markerActive ? 0f : 1f;
        markerActive = !markerActive;
        marker.color = new Color(1f, 1f, 1f, newAlpha);
        //LeanTween.value(gameObject, color => marker.color = color, marker.color, 
        //    new Color(1f, 1f, 1f, newAlpha), .15f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (active) ToggleMarker();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!markerActive && active) marker.color = new Color(1f, 1f, 1f, .3f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!markerActive) marker.color = new Color(1f, 1f, 1f, 0f);
    }
}
