using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EventManager
{
    private Dictionary<Enum, Action> events = new Dictionary<Enum, Action>();
    private Dictionary<Enum, Delegate> genericEvents = new Dictionary<Enum, Delegate>();

    public void Subscribe(Enum eventType, Action listener)
    {
        if (events.ContainsKey(eventType))
        {
            events[eventType] += listener;
        }
        else
        {
            events[eventType] = listener;
        }
    }

    public void Unsubscribe(Enum eventType, Action listener)
    {
        if (events.ContainsKey(eventType))
        {
            events[eventType] -= listener;
        }
    }

    public void Execute(Enum eventType)
    {
        if (events.ContainsKey(eventType))
        {
            events[eventType]?.Invoke();
        }
    }

    public void Subscribe<T>(Enum eventType, Action<T> listener)
    {
        if (genericEvents.ContainsKey(eventType))
        {
            genericEvents[eventType] = (Action<T>)genericEvents[eventType] + listener;
        }
        else
        {
            genericEvents[eventType] = listener;
        }
    }

    public void Unsubscribe<T>(Enum eventType, Action<T> listener)
    {
        if (genericEvents.ContainsKey(eventType))
        {
            genericEvents[eventType] = (Action<T>)genericEvents[eventType] - listener;
        }
    }

    public void Execute<T>(Enum eventType, T eventData)
    {
        if (genericEvents.ContainsKey(eventType) && genericEvents[eventType] is Action<T> action)
        {
            action.Invoke(eventData);
        }
    }
}