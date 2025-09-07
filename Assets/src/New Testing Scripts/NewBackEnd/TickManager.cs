using System;
using src.New_Testing_Scripts;
using UnityEngine;

public class TickManager : MonoBehaviour
{

   private void Awake()
   {
       ServiceLocator.Register(this);
   }

   void Start()
   {
   }

    // Update is called once per frame
    void Update()
    {
        
    }
}
