using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SingleTreasure : MonoBehaviour
{
    public Sprite emptySprite;
    public Sprite fullSprite;
    public Image im;

    public void SetFull()
    {
        im.sprite = fullSprite;
    }

    public void SetEmpty()
    {
        im.sprite = emptySprite;
    }
}
