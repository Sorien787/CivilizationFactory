using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class IListenerScriptableObject<T> : ScriptableObject where T : IListenerInterface
{
    HashSet<T> m_Listeners = new HashSet<T>();

    public void AddListener(T listener)
    {
        m_Listeners.Add(listener);
    }

    public void RemoveListener(T listener)
    {
        m_Listeners.Remove(listener);
    }

    public void ForEachListener(Action<T> listenerAction)
    {
        foreach (T listener in m_Listeners)
        {
            listenerAction(listener);
        }
    }
}

