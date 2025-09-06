using System;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocator
{
    private static readonly Dictionary<Type, object> Services = new();

    public static void Register<T>(T script)
    {
        var scriptType = typeof(T);
        if (Services.ContainsKey(scriptType))
            Debug.LogWarning($"Replacing script of type: {typeof(T)}!");
            
        Services[scriptType] = script;
    }

    public static T Get<T>()
    {
        var scriptType = typeof(T);
        if (Services.ContainsKey(scriptType))
            return (T)Services[scriptType];
            
        Debug.LogError("Could not find script you were looking for! Returning null.");
        return default;
    }

    public static void Unregister<T>()
    {
        var scriptType = typeof(T);
        if (Services.ContainsKey(scriptType))
            Services.Remove(scriptType);
    }

    public static void ClearServices() => Services.Clear();
}
