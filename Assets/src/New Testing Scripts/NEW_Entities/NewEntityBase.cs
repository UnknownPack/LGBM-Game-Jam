using System;
using System.Collections;
using System.Collections.Generic;
using src.New_Testing_Scripts.NEW_Entities;
using src.New_Testing_Scripts.TileMapTesting;
using UnityEngine;
using UnityEngine.Tilemaps;
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
        
        [SerializeField]float LifePoints, Damage;
        [SerializeField]private int MovementPoints;
        [SerializeField]private int ActionPoints;
        [SerializeField]private Vector2Int currentPosition;
        private GridSystem _gridSystem;
        private Coroutine currentCoroutine;

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

            Vector3Int position = _gridSystem.Tilemap.WorldToCell(transform.position);
            transform.position = _gridSystem.Tilemap.GetCellCenterWorld(position);
            Vector2Int gridPosition = new Vector2Int(position.x, position.y);
            NewNode node = _gridSystem.GetGrid[gridPosition];

            currentPosition = gridPosition;
            _gridSystem.UpdateEntityPosition(this, currentPosition);
        }

        // Update is called once per frame
        void Update()
        {
            if (_gridSystem == null)
            {
                Debug.LogError("No GridSystem found in ServiceLocator.");
                return;
            }
            
            if (currentCoroutine == null && Input.GetKeyDown(KeyCode.Mouse0))
            {
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mouseWorld2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);
                RaycastHit2D pointCheck = Physics2D.Raycast(mouseWorld2D, Vector2.zero);

                if (pointCheck.collider == null)
                {
                    Debug.LogWarning($"Tile not hit!");
                    return;
                }
                
                Vector3Int cellPos = _gridSystem.Tilemap.WorldToCell(pointCheck.point);
                Vector2Int gridPos = new Vector2Int(cellPos.x, cellPos.y);
                
                if (!_gridSystem.GetGrid.ContainsKey(gridPos))
                {
                    Debug.LogError("Node not in grid!");
                    return;
                }

                NewNode startNode = _gridSystem.GetGrid[currentPosition];
                NewNode endNode = _gridSystem.GetGrid[gridPos];
                
                List<NewNode> potentialPath = _gridSystem.GetPath(startNode, endNode);

                if (potentialPath.Count > MovementPoints)
                {
                    Debug.LogWarning("Not Enough Movement Points to get to Node");
                    return;
                }
                
                Debug.Log($"Starting at Node {startNode.GetGridPosition} at {startNode.GetRealPosition} \n" +
                          $"Ending at Node {endNode.GetGridPosition} at {endNode.GetRealPosition}");

                if (currentCoroutine == null)
                {
                    currentCoroutine = StartCoroutine(RunAction(ActionList[0].Action(endNode.GetNodeObject)));
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Destroy(GameObject.Find("Pointer"));
                ListenerManager.Notify("Tick");
                Debug.Log("Space Bar pressed!");
                if (currentCoroutine!=null)
                {
                    StopCoroutine(currentCoroutine);
                    currentCoroutine = null;
                } 
                MovementPoints = initialStats.MovementPoints;
                ActionPoints = initialStats.ActionPoints;
            }
        }
        
        private IEnumerator RunAction(IEnumerator routine)
        {
            yield return routine;
            currentCoroutine = null; // ✅ FIX: allow new clicks after move finishes
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
