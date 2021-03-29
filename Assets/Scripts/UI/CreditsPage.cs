using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsPage : MonoBehaviour
{
    public RectTransform panelRt;
    private bool tweening = false;
    public void OpenPanel()
    {
        if (!tweening)
        {
            panelRt.anchoredPosition = new Vector2(panelRt.anchoredPosition.x, -1024);
            tweening = true;
            LeanTween.move(panelRt, new Vector3(panelRt.anchoredPosition.x, 0f),1f)
                .setEaseOutQuart().setOnComplete(() => tweening = false);
        }
        
    }

    public void ClosePanel()
    {
        if (!tweening)
        {
            tweening = true;
            LeanTween.move(panelRt, new Vector3(panelRt.anchoredPosition.x, -1024),1f)
                .setEaseOutQuart().setOnComplete(()=>tweening = false); 
        }
        
    }
}