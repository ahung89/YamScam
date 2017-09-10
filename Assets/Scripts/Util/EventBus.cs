using UnityEngine;
using System.Collections.Generic;
using System;

public static class EventBus {

    private static Dictionary<Type, Dictionary<GameObject, List<Delegate>>> events 
        = new Dictionary<Type, Dictionary<GameObject, List<Delegate>>>();
    private static List<GameObject> cleanupList = new List<GameObject>();
    private static Type eventTypeBeingProcessed = null;

    public static void PublishEvent(object e, bool includeInactive = false)
    {
        Cleanup();

        Type t = e.GetType();

        if (!events.ContainsKey(t))
        {
            return;
        }

        Dictionary<GameObject, List<Delegate>> eventDict = events[t];

        eventTypeBeingProcessed = t;
        foreach (List<Delegate> funcs in eventDict.Values)
        {
            // right now global events only work for monobehaviours
            GameObject obj = (funcs[0].Target as MonoBehaviour).gameObject;

            if (obj == null)
            {
                cleanupList.Add(obj);
            }
            else if (includeInactive || obj.activeInHierarchy)
            {
                foreach (Delegate func in funcs)
                {
                    func.DynamicInvoke(e);
                }
            }
        }
        eventTypeBeingProcessed = null;
    }

    private static void Cleanup()
    {
        foreach (Dictionary<GameObject, List<Delegate>> dict in events.Values)
        {
            foreach(GameObject go in cleanupList)
            {
                if (dict.ContainsKey(go))
                {
                    dict.Remove(go);
                }
            }
        }
        cleanupList.Clear();
    }

    public static void Subscribe<T> (Action<T> func) where T : struct
    {
        Subscribe(typeof(T), func);
    }

    public static void Subscribe (Type type, Delegate func)
    {
        if (!events.ContainsKey(type))
        {
            events[type] = new Dictionary<GameObject, List<Delegate>>();
        }

        GameObject target = (func.Target as MonoBehaviour).gameObject;
        if (!events[type].ContainsKey(target))
        {
            events[type][target] = new List<Delegate>();
        }

        events[type][target].Add(func);
    }

    public static void Subscribe<T>(this GameObject obj, Action<T> func)
    {
        obj.GetComponent<EventSubscriber>().Subscribe<T>(func);
    }

    public static void Unsubscribe<T>(Action<T> func)
    {
        Type type = typeof(T);
        if (events.ContainsKey(typeof(T)))
        {
            GameObject obj = (func.Target as MonoBehaviour).gameObject;
            if (events[type].ContainsKey(obj))
            {
                if (eventTypeBeingProcessed == type)
                {
                    cleanupList.Add(obj);
                }
                else
                {
                    events[type][obj].Remove(func);
                }
            }
        }
    }

    public static void UnsubscribeAll(GameObject obj)
    {
        cleanupList.Add(obj);
    }

    public static void Unsubscribe<T>(this GameObject obj, Action<T> func)
    {
        obj.GetComponent<EventSubscriber>().Unsubscribe<T>(func);

    }

	public static void PublishEvent(this GameObject go, object e)
    {
        if (go.activeInHierarchy)
        {
            go.GetComponent<EventSubscriber>().PublishEvent(e);
        }
    }

    public static void Reset()
    {
        events.Clear();
        eventTypeBeingProcessed = null;
        cleanupList.Clear();
    }
}
