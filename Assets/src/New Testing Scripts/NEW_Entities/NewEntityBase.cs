using System;
using System.Collections.Generic;
using src.New_Testing_Scripts.NEW_Entities;
using src.New_Testing_Scripts.TileMapTesting;
using UnityEngine;
using MoveAction = src.New_Testing_Scripts.NEW_Entities.MoveAction;

namespace src.New_Testing_Scripts
{
    public class NewEntityBase : MonoBehaviour
    {
        [SerializeField]private List<NewStatusEffect> statusEffects = new List<NewStatusEffect>();
        [SerializeField] private InitialStats initialStats;

        public class Entity
        {
            private Vector2Int gridPosition;
 
        }

        private List<NewAction> ActionList = new List<NewAction>();
        
        float LifePoints, Damage;
        private int MovementPoints;
        private int ActionPoints;
        private Vector2Int currentPosition;
        private GridSystem _gridSystem;

        private void Start()
        {
            LifePoints = initialStats.LifePoints;
            MovementPoints = initialStats.MovementPoints;
            ActionPoints = initialStats.ActionPoints;
            Damage = initialStats.Damage;
            
            ActionList.Add(new NEW_Entities.MoveAction(this));
            
            var tickManager = ServiceLocator.Get<ListenerManager>();
            if (tickManager == null)
            {
                Debug.LogError("There is no listener manager for status effect.");
                return;
            }
            tickManager.AddListener("Tick", OnTick);

            _gridSystem = ServiceLocator.Get<GridSystem>();
            if (_gridSystem == null)
            {
                Debug.LogError("There is no listener manager for status effect.");
                return;
            }

            NewNode currentNode = _gridSystem.GetNodeAtTarget(this.gameObject);
            gameObject.transform.position = currentNode.GetRealPosition;
            currentPosition = currentNode.GetGridPosition;
            _gridSystem.UpdateEntityPosition(this, currentPosition);
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

        public float GetHealth
        {
            get => LifePoints;
            set => LifePoints = value;
        }
        
        public float GetDamage
        {
            get => Damage;
            set => Damage = value;
        }

        public int GetMovementPoints
        {
            get => MovementPoints;
            set => MovementPoints = value;
        }

        public int GetActionPoints => ActionPoints;
        public Vector2Int GridPosition
        {
            get => currentPosition;
            set
            {
                if (value == currentPosition) return;
                currentPosition = value;
                _gridSystem.UpdateEntityPosition(this, currentPosition);
            }
        }

        public List<NewStatusEffect> GetStatusEffect => statusEffects;

        public bool ContainsEffeccct(StatusEffectName name)
        {
            if (statusEffects.Count <= 0)
                return false;

            foreach (var effect in statusEffects)
            {
                if (effect.GetStatusEffectName == name)
                    return true;
            }

            return false;
        }

        public InitialStats GetInitialStats => initialStats;

    }
    
    
}

public struct InitialStats
{
    public float LifePoints;
    public float Damage;
    public int MovementPoints;
    public int ActionPoints;
}
