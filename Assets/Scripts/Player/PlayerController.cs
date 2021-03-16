using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Grid grid;
    private Fogmap fogmap;
    private Vector2Int playerPos;
    private BoatDir direction = BoatDir.Right;
    private float timer;
    private int currentFrame = 0;
    private bool moving = false;
    public float moveTime = .5f;
    public SpriteRenderer sr;
    public float framerate = .5f;
    public List<AnimationFrames> animationSprites;

    void Start()
    {
        
    }

    public void SetGridAndPlayer(Grid grid, Vector2Int pos, Fogmap fogmap)
    {
        this.grid = grid;
        this.playerPos = pos;
        this.fogmap = fogmap;
        fogmap.DeactivateSquare(playerPos);
    }

    void Update()
    {
        if (!moving)
        {
            CheckMovement();
        }
        HandleAnimation();
    }

    private void HandleAnimation()
    {
        timer += Time.deltaTime;

        if (timer >= framerate)
        {
            timer -= framerate;
            currentFrame = currentFrame == 1 ? 0 : 1;
            sr.sprite = animationSprites[(int) direction].sprites[currentFrame];
        }
    }

    private void CheckMovement()
    {
        float dist = grid.GetCellSize();
        bool hasMoved = false;
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (playerPos.y < grid.GetHeight() - 1)
            {
                playerPos.y += 1;
                LeanTween.moveLocal(gameObject, transform.position + new Vector3(0f, dist, 0f), moveTime)
                    .setOnComplete(() => moving = false);
                direction = BoatDir.Up;
                hasMoved = true;
            }
        } else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (playerPos.x > 0)
            {
                playerPos.x -= 1;
                LeanTween.moveLocal(gameObject, transform.position + new Vector3(-dist, 0f, 0f), moveTime)
                    .setOnComplete(() => moving = false);
                direction = BoatDir.Left;
                hasMoved = true;
            }
        } else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (playerPos.y > 0)
            {
                playerPos.y -= 1;
                LeanTween.moveLocal(gameObject, transform.position + new Vector3(0f, -dist, 0f), moveTime)
                    .setOnComplete(() => moving = false);
                direction = BoatDir.Down;
                hasMoved = true;
            }
        } else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (playerPos.x < grid.GetWidth() - 1)
            {
                playerPos.x += 1;
                LeanTween.moveLocal(gameObject, transform.position + new Vector3(dist, 0f, 0f), moveTime)
                    .setOnComplete(() => moving = false);
                direction = BoatDir.Right;
                hasMoved = true;
            }
        }

        if (hasMoved)
        {
            moving = true;
            sr.sprite = animationSprites[(int) direction].sprites[currentFrame];
            fogmap.DeactivateSquare(playerPos);
        }
    }
}

public enum BoatDir
{
    Up,
    Down,
    Left,
    Right
}

[System.Serializable]
public struct AnimationFrames
{
    public Sprite[] sprites;
}
