using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPanel : MonoBehaviour
{
    public RectTransform panelRt;

    public void OpenPanel()
    {
        gameObject.SetActive(true);
        panelRt.anchoredPosition = new Vector2(panelRt.anchoredPosition.x, -700f);
        LeanTween.move(panelRt, new Vector3(panelRt.anchoredPosition.x, 0f),1f)
            .setEaseOutQuart();
    }

    public void ReloadLevel()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}
