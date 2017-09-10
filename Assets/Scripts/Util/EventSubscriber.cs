using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class EventSubscriber : MonoBehaviour {

    private Dictionary<Type, List<Delegate>> events = new Dictionary<Type, List<Delegate>>();
    private List<Delegate> cleanupList = new List<Delegate>();
    private bool eventInProgress = false;

    public void PublishEvent(object obj, bool includeInactive = false)
    {
        cleanupList.Clear();
        Type type = obj.GetType();

        if (!events.ContainsKey(type))
        {
            return;
        }

        List<Delegate> funcs = events[type];

        eventInProgress = true;
        foreach (Delegate func in funcs)
        {
            bool isMonoBehaviour = func.Target is MonoBehaviour;
            if (func.Target == null)
            {
                cleanupList.Add(func);
                return;
            }

            if (!(func.Target is MonoBehaviour))
            {
                func.DynamicInvoke(obj);
            }
            else
            {
                MonoBehaviour mb = func.Target as MonoBehaviour;
                if (mb.gameObject == null)
                {
                    cleanupList.Add(func);
                }
                else if (includeInactive || (mb.gameObject.activeInHierarchy && mb.enabled))
                {
                    func.DynamicInvoke(obj);
                }
            }
        }

        eventInProgress = false;

        foreach (Delegate del in cleanupList)
        {
            events[type].Remove(del);
        }
    }

    void Awake()
    {
        MonoBehaviour[] monoBehaviours = GetComponents<MonoBehaviour>();

        foreach (MonoBehaviour monoBehaviour in monoBehaviours)
        {
            RegisterCallbacks(monoBehaviour);
        }
    }

    private void OnDestroy ()
    {
        EventBus.UnsubscribeAll(gameObject);
    }

    public void Subscribe<T>(Delegate del)
    {
        AddFunction(typeof(T), del);
    }

    public void Unsubscribe<T>(Delegate del)
    {
        Type type = typeof(T);
        if (events.ContainsKey(type))
        {
            if (eventInProgress)
            {
                cleanupList.Add(del);
            } else
            {
                events[type].Remove(del);
            }
        }
    }

    public void RegisterCallbacks(MonoBehaviour monoBehaviour)
    {
        MethodInfo[] methods = monoBehaviour.GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic
            | BindingFlags.Instance);
        foreach (MethodInfo method in methods)
        {
            Attribute[] attributes = Attribute.GetCustomAttributes(method);
            foreach (Attribute attribute in attributes)
            {
                if (attribute is Subscribe)
                {
                    Type paramType = method.GetParameters()[0].ParameterType;
                    Delegate del = Delegate.CreateDelegate(Expression.GetActionType(paramType),
                        monoBehaviour, method.Name);
                    AddFunction(paramType, del);
                }
                else if (attribute is SubscribeGlobal)
                {
                    Type paramType = method.GetParameters()[0].ParameterType;
                    Delegate del = Delegate.CreateDelegate(Expression.GetActionType(paramType),
                        monoBehaviour, method.Name);
                    EventBus.Subscribe(paramType, del);
                }
            }
        }
    }

    private void AddFunction(Type type, Delegate del)
    {
        if (!events.ContainsKey(type))
        {
            events[type] = new List<Delegate>();
        }
        events[type].Add(del);
    }
}
