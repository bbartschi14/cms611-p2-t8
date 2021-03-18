using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Events/Vector2Int Game Event")]

public class Vector2IntGameEvent : ScriptableObject
{
    private readonly List<Vector2IntGameEventListener> eventListeners =
        new List<Vector2IntGameEventListener>();

    public void Raise(Vector2Int vec2)
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
        {
            eventListeners[i].OnEventRaised(vec2);
        }
    }

    public void RegisterListener(Vector2IntGameEventListener listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void UnregisterListener(Vector2IntGameEventListener listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }

}