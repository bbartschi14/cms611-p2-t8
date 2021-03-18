using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Vector2IntGameEventListener : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public Vector2IntGameEvent Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public UnityEvent<Vector2Int> Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(Vector2Int vec2)
    {
        Response.Invoke(vec2);
    }
}