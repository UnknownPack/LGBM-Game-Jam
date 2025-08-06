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
    
    private Dictionary<ActionType, int> ActionPoints = new Dictionary<ActionType, int>
    {
        { ActionType.MovePoint, 1},{ ActionType.ActionPoint, 1 },
    };
    private List<ActionBase> Abilities = new List<ActionBase>();
    private Dictionary<Vector2Int, Node> Grid;
     private Node CurrentNode;
 
    
    private Animator EntityAnimtor;
    private GridManager _gridManager;
    private PathFinding _pathFinding;

    public virtual void InitActions(ActionBase action, float ActionRange, int CostOfAction)
    {
        action.Init(gameObject, this, ActionRange, CostOfAction,  ActionType.MovePoint, _gridManager);
        Abilities.Add(action);
        Debug.Log("Ability Initialized");
    }
    
    void Start()
    {
        EntityAnimtor = GetComponent<Animator>();
        
    }

    void Update()
    {
        
    }

    #region Public Getters
        public float GetHealth => Health;
        public float GetDamage => Damage;
        public float GetDefence => Defence;
        public float GetMoveSpeed => MoveSpeed;
        public Animator GetAnimator => EntityAnimtor;
        public List<ActionBase> GetAbilityList => Abilities;
    #endregion
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
        
        InitActions(new MoveAction(), MoveSpeed, MovePoint_Cost);
    }

    public void ResetActionPoints()
    {
        ActionPoints[ActionType.MovePoint] = MovePoint_MaxCount;
        ActionPoints[ActionType.ActionPoint] = ActionPoint_MaxCount;
    }

    public bool isActionPointAvailable(ActionType actionType) => ActionPoints[actionType] > 0;

    public void RemoveActionPoint(ActionType actionType, int amount) => ActionPoints[actionType] -= amount;
}

[System.Serializable]
public enum UnitOwnership
{
    Player,
    Ally,
    Enemy
}
