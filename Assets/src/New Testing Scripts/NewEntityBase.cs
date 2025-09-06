using System;
using System.Collections.Generic;
using UnityEngine;

namespace src.New_Testing_Scripts
{
    public class NewEntityBase : MonoBehaviour
    {
        [SerializeField]private List<NewStatusEffect> statusEffects = new List<NewStatusEffect>();
        [SerializeField] private InitialStats initialStats;
        private float LifePoints;
        private int MovementPoints;
        private int ActionPoints;

        private void Start()
        {
            LifePoints = initialStats.LifePoints;
            MovementPoints = initialStats.MovementPoints;
            ActionPoints = initialStats.ActionPoints;
            
            var tickManager = ServiceLocator.Get<ListenerManager>();
            
            if (tickManager == null)
            {
                Debug.LogError("There is no listener manager for status effect.");
                return;
            }
            tickManager.AddListener("Tick", OnTick);
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnTick()
        {
            ManageStatusEffects();
        }
        
        private void ManageStatusEffects()
        {
            for (int i = statusEffects.Count - 1; i >= 0; i--)
            {
                NewStatusEffect effect = statusEffects[i];
                if (!effect.IsActive)
                {
                    Debug.Log($"Removing inactive status effect: {effect.GetStatusEffectName} from {gameObject.name}");
                    statusEffects.RemoveAt(i);
                }
            }
        }

        public void AddStatusEffect(NewStatusEffect newStatusEffect)
        {
            statusEffects.Add(newStatusEffect);
        }
    }
    
    
}

public struct InitialStats
{
    public float LifePoints;
    public int MovementPoints;
    public int ActionPoints;
}
