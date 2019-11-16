using System.Collections;
using System.Collections.Generic;
using System;

public class GameEvent
{
}

public class EventBus
{
    static EventBus s_instance = null;
    public static EventBus Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = new EventBus();
            }

            return s_instance;
        }
    }

    public delegate void EventDelegate<T> (T e) where T : GameEvent;
    private delegate void EventDelegate(GameEvent e);

    private Dictionary<Type, EventDelegate> m_delegates = new Dictionary<Type, EventDelegate>();
    private Dictionary<Delegate, EventDelegate> m_delegateLookup = new Dictionary<Delegate, EventDelegate>();

    public void AddListener<T> (EventDelegate<T> del) where T : GameEvent
    {
        if (m_delegateLookup.ContainsKey(del))
        {
            return;
        }

        EventDelegate internalDelegate = (e) => del((T)e);
        m_delegateLookup[del] = internalDelegate;
        EventDelegate tempDel;

        if (m_delegates.TryGetValue(typeof(T), out tempDel))
        {
            m_delegates[typeof(T)] = tempDel += internalDelegate;
        }
        else
        {
            m_delegates[typeof(T)] = internalDelegate;
        }
    }

    public void RemoveListener<T> (EventDelegate<T> del) where T : GameEvent
    {
        EventDelegate internalDelegate;

        if (m_delegateLookup.TryGetValue(del, out internalDelegate))
        {
            EventDelegate tempDel;

            if (m_delegates.TryGetValue(typeof(T), out tempDel))
            {
                tempDel -= internalDelegate;

                if (tempDel == null)
                {
                    m_delegates.Remove(typeof(T));
                }
                else
                {
                    m_delegates[typeof(T)] = tempDel;
                }
            }

            m_delegateLookup.Remove(del);
        }
    }

    public void Raise(GameEvent e)
    {
        EventDelegate del;

        if (m_delegates.TryGetValue(e.GetType(), out del))
        {
            del.Invoke(e);
        }
    }
}
