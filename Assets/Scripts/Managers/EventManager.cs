/*
* Copyright 2017 Ben D'Angelo
*
* MIT License
*
* Permission is hereby granted, free of charge, to any person obtaining a copy of this
* software and associated documentation files (the "Software"), to deal in the Software
* without restriction, including without limitation the rights to use, copy, modify, merge,
* publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
* to whom the Software is furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in all copies or
* substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED *AS IS*, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
* INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
* PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
* FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
* OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
* DEALINGS IN THE SOFTWARE.
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventManager : MonoBehaviour // Took from https://gist.github.com/bendangelo/093edb33c2e844c5c73a
{
    private static EventManager instance;
    public static EventManager Instance {
        get
        {
            return instance;
        }
    }

    public delegate void EventDelegate<T>(T e) where T : GameEvent;
    private delegate void EventDelegate(GameEvent e);

    private Dictionary<System.Type, EventDelegate> delegates = new Dictionary<System.Type, EventDelegate>();
    private Dictionary<System.Delegate, EventDelegate> delegateLookup = new Dictionary<System.Delegate, EventDelegate>();

    private EventDelegate AddDelegate<T>(EventDelegate<T> del) where T : GameEvent
    {
        // Early-out if we've already registered this delegate
        if (delegateLookup.ContainsKey(del))
            return null;

        // Create a new non-generic delegate which calls our generic one.
        // This is the delegate we actually invoke.
        EventDelegate internalDelegate = (e) => del((T)e);
        delegateLookup[del] = internalDelegate;

        EventDelegate tempDel;
        if (delegates.TryGetValue(typeof(T), out tempDel)) {
            delegates[typeof(T)] = tempDel += internalDelegate;
        } else {
            delegates[typeof(T)] = internalDelegate;
        }

        return internalDelegate;
    }


    private void Awake()
    {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void AddListener<T>(EventDelegate<T> del) where T : GameEvent
    {
        AddDelegate<T>(del);
    }

    public void RemoveListener<T>(EventDelegate<T> del) where T : GameEvent
    {
        EventDelegate internalDelegate;
        if (delegateLookup.TryGetValue(del, out internalDelegate)) {
            EventDelegate tempDel;
            if (delegates.TryGetValue(typeof(T), out tempDel)) {
                tempDel -= internalDelegate;
                if (tempDel == null) {
                    delegates.Remove(typeof(T));
                } else {
                    delegates[typeof(T)] = tempDel;
                }
            }

            delegateLookup.Remove(del);
        }
    }

    public void RemoveAll()
    {
        delegates.Clear();
        delegateLookup.Clear();
    }

    public bool HasListener<T>(EventDelegate<T> del) where T : GameEvent
    {
        return delegateLookup.ContainsKey(del);
    }

    public void TriggerEvent(GameEvent e)
    {
        EventDelegate del;
        if (delegates.TryGetValue(e.GetType(), out del)) {
            del.Invoke(e);

        }
    }

    public void OnApplicationQuit()
    {
        RemoveAll();
    }
}