using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class BaseBattleEntity : MonoBehaviour
{
    [Header("Entity Stats")]
    [SerializeField] protected float Health;
    [SerializeField] protected float Maxhealth;
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

    [Header("Entity UI")]
    [SerializeField] private TextMeshProUGUI DamageTxt;
    [SerializeField] private TextMeshProUGUI HealTxt;
    [SerializeField] private HealthBar Healthbar;
    [SerializeField] private Animator Animator;
    

    private Dictionary<ActionType, int> ActionPoints;
    private Dictionary<AbilityName, ActionBase> Abilities = new Dictionary<AbilityName, ActionBase>();
    private Dictionary<Vector2Int, Node> Grid;
    private Node CurrentNode;
    private InitalEntityStats initalEntityStats;
    private GameObject grenadePrefab;
    
    private Animator EntityAnimtor;
    protected GridManager _gridManager;
    private PathFinding _pathFinding;
    private TurnManager _turnManager;

    public const float DefenceReductionFactor = 0.025f;

    private void Awake()
    {
        Healthbar = GetComponentInChildren<HealthBar>(); 
        Animator = GetComponentInChildren<Animator>();
    }
    
    void Start()
    {
        ActionPoints = new Dictionary<ActionType, int>
        {
            { ActionType.MovePoint, MovePoint_MaxCount},{ ActionType.ActionPoint, ActionPoint_MaxCount },
        };
        EntityAnimtor = GetComponent<Animator>();
        initalEntityStats = new InitalEntityStats(Health, Maxhealth, Damage, Defence, MoveSpeed);
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
        transform.position = _gridManager.GetNodeFromPosition(transform.localPosition).GetRealPosition;
        Debug.Log(_gridManager.GetNodeFromPosition(transform.position).GetRealPosition + " " + _gridManager.GetNodeFromPosition(transform.position).GetGridPosition);
        _pathFinding = _gridManager.GetPathFinding;
        Grid = _gridManager.GetGridNodes;

        InitialiseActions();
    }

    protected virtual void InitialiseActions() =>
        InitActions(AbilityName.Move, new MoveAction(), MoveSpeed, MovePoint_Cost, ActionType.MovePoint, ActionTargetType.Tile);
    
    
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
        public GridManager GetGridManager => _gridManager;

    public virtual void TakeDamage(float damageAmount)
    {
        float r = Defence * DefenceReductionFactor;     // % reduction (can be negative)
        float multiplier = Mathf.Max(0f, 1f - r);       // never less than 0
        float finalDamage = damageAmount * multiplier;  // can be > damageAmount if r < 0
        Debug.Log($"Taking damage: {finalDamage} (original: {damageAmount}, defence: {Defence}, reduction factor: {DefenceReductionFactor})");
        Health -= finalDamage;
        Debug.Log($"Health: {Health}");
        Healthbar.UpdateHealthBar(Health, Maxhealth);
        if (Health <= 0)
            Death();
    }

        public void Heal(float amountToHeal)
        {
            Health += amountToHeal;
            Health = Mathf.Clamp(Health, 0, Maxhealth);
            Healthbar.UpdateHealthBar(Health, Maxhealth);
        }
        
        public virtual float GetDamage () => Damage;
        public float GetDefence => Defence;
        public float GetMoveSpeed => MoveSpeed;
        public Animator GetAnimator => Animator;
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

        public InitalEntityStats GetIntialStats => initalEntityStats;
        public void SetDefence(float value) => Defence = value;
        
        public void SetGrenadePrefab(GameObject prefab) => grenadePrefab = prefab;
        public GameObject GetGrenadePrefab() => grenadePrefab;
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
    public float Maxhealth;
    public float Damage;
    public float Defence;
    public int MoveSpeed;

    public InitalEntityStats(float health, float maxHealth, float damage, float defence, int moveSpeed)
    {
        Health = health;
        Maxhealth = maxHealth;
        Damage = damage;
        Defence = defence;
        MoveSpeed = moveSpeed;
    }
}
