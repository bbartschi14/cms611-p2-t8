using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTentacle : MonoBehaviour
{
    private int tentacleID;
    public SpriteRenderer tentacleSR;
    public List<AnimationFrames> tentacleSprites;
    public void SetID(int id)
    {
        tentacleID = id;
        tentacleSR.sprite = tentacleSprites[tentacleID].sprites[0];
        if (tentacleID < 3)
        {
            tentacleSR.sortingOrder = 5;
        }
    }

    public void RemoveSelf()
    {
        StartCoroutine(StartAnimation(tentacleSprites[tentacleID].sprites, .3f));
    }
    
    IEnumerator StartAnimation(Sprite[] sprites, float time)
    {
        foreach (Sprite frame in sprites)
        {
            tentacleSR.sprite = frame;
            yield return new WaitForSeconds(time / sprites.Length);
        }
        Destroy(gameObject);
        yield break;
    }
}
