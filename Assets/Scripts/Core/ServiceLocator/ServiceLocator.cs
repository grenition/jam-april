using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocator
{
    private static Dictionary<Type, object> _services = new();

    public static void Register<T>(T service)
    {
        var key = typeof(T);
        if (_services.ContainsKey(key))
        {
            Debug.LogError(
                $"Attempted to register service of type {key} which is already registered with the {key.Name}.");
            return;
        }

        _services.Add(key, service);
    }
    public static void Unregister<T>()
    {
        var key = typeof(T);
        if (!_services.ContainsKey(key))
        {
            Debug.LogError(
                $"Attempted to unregister service of type {key} which is not registered with the {key.Name}.");
            return;
        }

        _services.Remove(key);
    }
    public static T Get<T>()
    {
        var key = typeof(T);
        if (!_services.ContainsKey(key))
        {
            Debug.LogError($"{key} not registered with {key.Name}");
            return default;
        }

        return (T)_services[key];
    }
}
