using System;
using System.Collections.Generic;
using UnityEngine;

namespace src.New_Testing_Scripts
{
    public static class ListenerManager 
    {
        private static Dictionary<string, Action> listeners = new Dictionary<string, Action>();

        public static void AddListener(string listenerId, Action callback)
        {
            bool exists = listeners.ContainsKey(listenerId);
            listeners[listenerId] = exists? listeners[listenerId] + callback : callback;
        }

        public static void RemoveListener(string listenerId, Action callback)
        {
            if (listeners.ContainsKey(listenerId))
            {
                listeners[listenerId] -= callback;

                if (listeners[listenerId] == null)
                    listeners.Remove(listenerId);
            }
        }

        public static void Notify(string listenerId)
        {
            if (listeners.ContainsKey(listenerId))
            {
                listeners[listenerId].Invoke();
                Debug.LogWarning($"{listenerId} called !");
            }
            else
                Debug.LogWarning($"Listener {listenerId} not found!");
        }
    }
}
