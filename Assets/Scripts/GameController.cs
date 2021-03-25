using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Texture2D cursorTex;
    void Start()
    {
        Cursor.SetCursor(cursorTex, Vector2.up * 50, CursorMode.ForceSoftware);
    }

    
}
