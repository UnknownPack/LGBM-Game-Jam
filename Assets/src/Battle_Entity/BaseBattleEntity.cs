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
    private Dictionary<AbilityName, ActionBase> Abilities = new Dictionary<AbilityName, ActionBase>();
    private Dictionary<Vector2Int, Node> Grid;
    private List<StatusEffects> StatusEffects;
    private Node CurrentNode;
    private InitalEntityStats initalEntityStats;
    
    private Animator EntityAnimtor;
    private GridManager _gridManager;
    private PathFinding _pathFinding;
    private TurnManager _turnManager;
    
    void Start()
    {
        StatusEffects = new List<StatusEffects>();
        ActionPoints = new Dictionary<ActionType, int>
        {
            { ActionType.MovePoint, MovePoint_MaxCount},{ ActionType.ActionPoint, ActionPoint_MaxCount },
        };
        EntityAnimtor = GetComponent<Animator>();
        initalEntityStats = new InitalEntityStats(Health, Damage, Defence, MoveSpeed);
    }

    public void InjectGridManager(GridManager gridManager, TurnManager turnManager)
    {
        _gridManager = gridManager;
        _gridManager.AddEntityToGrid(this);
        _turnManager = turnManager;
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
        InitActions(AbilityName.Move, new MoveAction(), MoveSpeed, MovePoint_Cost, ActionType.MovePoint, ActionTargetType.Tile);
    }
    
    public void InitActions(AbilityName name,ActionBase action, float actionRange, int costOfAction, ActionType actionType, ActionTargetType actionTargetType)
    {
        action.Init(gameObject, this, actionRange, costOfAction, actionTargetType, actionType, _gridManager);
        Abilities.Add(name, action);
    }

    protected virtual void Death()
    {
        _gridManager.RemoveEntityFromGrid(this);
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
        public void SetCurrentNode(Node node) => CurrentNode = node;
        public Dictionary<AbilityName, ActionBase> GetAbilityList => Abilities;
        public Dictionary<ActionType, int> GetActionPoints => ActionPoints;
        public int GetActionPointsCount(ActionType actionType) => ActionPoints[actionType]; 
        
        public void RemoveActionPoint(ActionType actionType, int amount) => ActionPoints[actionType] -= amount;
        
        public void ResetActionPoints()
        {
            ActionPoints[ActionType.MovePoint] = MovePoint_MaxCount;
            ActionPoints[ActionType.ActionPoint] = ActionPoint_MaxCount;
        }
        
        public void AddStatusEffect(StatusEffects statusEffect) => StatusEffects.Add(statusEffect);
        public List<StatusEffects> GetStatusEffects() => StatusEffects;

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

public struct InitalEntityStats
{
    public float Health;
    public float Damage;
    public float Defence;
    public int MoveSpeed;

    public InitalEntityStats(float health, float damage, float defence, int moveSpeed)
    {
        Health = health;
        Damage = damage;
        Defence = defence;
        MoveSpeed = moveSpeed;
    }
}
