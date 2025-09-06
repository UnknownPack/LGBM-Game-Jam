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
        [SerializeReference] private List<NewStatusEffect> statusEffects = new List<NewStatusEffect>();
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
            ListenerManager.AddListener("Tick", OnTick);

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
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                GameObject previousParent = GameObject.Find("ParentObject");
                if (previousParent != null)
                    DestroyImmediate(previousParent);

                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mouseWorld2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

                RaycastHit2D pointCheck = Physics2D.Raycast(mouseWorld2D, Vector2.zero);

                if (pointCheck.collider != null)  
                {
                    Vector3Int cellPos = Tilemap.WorldToCell(pointCheck.point);
                    Vector2Int gridPos = new Vector2Int(cellPos.x, cellPos.y);

                    if (Grid.ContainsKey(gridPos))
                    {
                        List<Vector2Int> keys = new List<Vector2Int>(pathfinder.GetGrid.Keys);
                        Vector2Int randomStartPos = keys[Random.Range(0, keys.Count)];

                        NewNode startNode = pathfinder.GetGrid[randomStartPos];
                        NewNode endNode = pathfinder.GetGrid[gridPos];

                        List<NewNode> path = pathfinder.GetPath(startNode, endNode);

                        PrintPath(path);
                    }
                    else
                        Debug.LogWarning("Clicked cell is outside of painted Tilemap!");
                }
                else
                    Debug.LogWarning("Click did not hit TilemapCollider2D");
            }
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
            Debug.Log($"{newStatusEffect} applied to{gameObject.name}");
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

[Serializable]
public struct InitialStats
{
    public float LifePoints;
    public float Damage;
    public int MovementPoints;
    public int ActionPoints;
}
