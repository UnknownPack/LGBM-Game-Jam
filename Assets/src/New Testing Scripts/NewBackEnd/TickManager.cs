using System;
using src.New_Testing_Scripts;
using UnityEngine;

public class TickManager : MonoBehaviour
{
   private ListenerManager listenerManager = new ListenerManager();

   private void Awake()
   {
       ServiceLocator.Register(this);
       ServiceLocator.Register(listenerManager);
   }

   void Start()
   {
       listenerManager.Notify("Tick");
   }

    // Update is called once per frame
    void Update()
    {
        
    }
}
