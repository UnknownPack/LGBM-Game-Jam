using System;
using System.Collections.Generic;
using UnityEngine;

namespace src.New_Testing_Scripts
{
    public class ListenerManager 
    {
        private Dictionary<string, Action> listeners = new Dictionary<string, Action>();

        public void AddListener(string listenerId, Action callback)
        {
            bool exists = listeners.ContainsKey(listenerId);
            listeners[listenerId] = exists? listeners[listenerId] + callback : callback;
        }

        public void RemoveListener(string listenerId, Action callback)
        {
            if (listeners.ContainsKey(listenerId))
            {
                listeners[listenerId] -= callback;

                if (listeners[listenerId] == null)
                    listeners.Remove(listenerId);
            }
        }

        public void Notify(string listenerId)
        {
            if (listeners.ContainsKey(listenerId))
                listeners[listenerId].Invoke(); 
            else
                Debug.LogWarning($"Listener {listenerId} not found!");
        }
    }
}
