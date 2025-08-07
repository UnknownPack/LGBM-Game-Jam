using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BaseBattleEntity : MonoBehaviour
{
    [Header("Entity Stats")]
    [SerializeField] protected float Health;
    [SerializeField] protected float Damage;
    [SerializeField] protected float Defence;
    [SerializeField, Tooltip("Dictates the amount of tiles the entity can travel")] private int MoveSpeed;

    [Header("Entity Action Points")] 
    [SerializeField]protected int MovePoint_MaxCount = 1;
    [SerializeField]protected int MovePoint_Cost = 1;
    [SerializeField]protected int ActionPoint_MaxCount = 1;
    [SerializeField]protected int ActionPoint_Cost = 1;

    [Header("Entity Alligence")]
    [SerializeField]protected UnitOwnership UnitOwner;

    private Dictionary<ActionType, int> ActionPoints;
    private List<ActionBase> Abilities = new List<ActionBase>();
    private Dictionary<Vector2Int, Node> Grid;
     private Node CurrentNode;
 
    
    private Animator EntityAnimtor;
    private GridManager _gridManager;
    private PathFinding _pathFinding;
    
    void Start()
    {
        ActionPoints = new Dictionary<ActionType, int>
        {
            { ActionType.MovePoint, 1},{ ActionType.ActionPoint, 1 },
        };
        EntityAnimtor = GetComponent<Animator>();
        
    }

    void Update()
    {
        
    }
    public void InjectGridManager(GridManager gridManager)
    {
        _gridManager = gridManager;
        if(_gridManager == null)
        {
            Debug.LogError("GridManager not found in the scene. Please ensure it is present.");
            return;
        }
        transform.position = _gridManager.GetNodeFromPosition(transform.position).GetRealPosition;
        _pathFinding = _gridManager.GetPathFinding;
        Grid = _gridManager.GetGridNodes;

        InitialiseActions();
    }

    protected virtual void InitialiseActions()
    {
        InitActions(new MoveAction(), MoveSpeed, MovePoint_Cost, ActionType.MovePoint, ActionTargetType.Tile);
    }
    
    private void InitActions(ActionBase action, float actionRange, int costOfAction, ActionType actionType, ActionTargetType actionTargetType)
    {
        action.Init(gameObject, this, actionRange, costOfAction, actionTargetType, actionType, _gridManager);
        Abilities.Add(action);
        Debug.Log("Ability Initialized");
    }

    protected virtual void Death()
    {
        Debug.Log($"{gameObject.name} has died!");
        Destroy(gameObject);
    }

    #region Public Helper Functions
        public float GetHealth => Health;
        
        public void TakeDamage(float damageAmount)
        {
            Health -= damageAmount;
            if (Health <= 0)
                Death();
        }

        public void Heal(float amountToHeal) => Health += amountToHeal;
        
        public float GetDamage => Damage;
        public float GetDefence => Defence;
        public float GetMoveSpeed => MoveSpeed;
        public Animator GetAnimator => EntityAnimtor;
        public List<ActionBase> GetAbilityList => Abilities;
        public Dictionary<ActionType, int> GetActionPoints => ActionPoints;
        public int GetActionPointsCount(ActionType actionType) => ActionPoints[actionType]; 
        
        public void RemoveActionPoint(ActionType actionType, int amount) => ActionPoints[actionType] -= amount;
        
        public void ResetActionPoints()
        {
            ActionPoints[ActionType.MovePoint] = MovePoint_MaxCount;
            ActionPoints[ActionType.ActionPoint] = ActionPoint_MaxCount;
        }

        public UnitOwnership GetUnitOwnerShip => UnitOwner;
        public bool isActionPointAvailable(ActionType actionType) => ActionPoints[actionType] > 0;
    
    #endregion
    
}

[System.Serializable]
public enum UnitOwnership
{
    Player,
    Ally,
    Enemy
}
