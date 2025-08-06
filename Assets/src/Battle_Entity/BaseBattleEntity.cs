using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BaseBattleEntity : MonoBehaviour
{
    [Header("Entity Stats")]
    [SerializeField] private float Health;
    [SerializeField] private float Damage;
    [SerializeField] private float Defence;
    [SerializeField, Tooltip("Dictates the amount of tiles the entity can travel")] private int MoveSpeed;

    private int ActionPoint = 1, MovePoint = 1, AvailablePoints;
    
    [Header("Entity Abilities")]
    private List<ActionBase> Abilities = new List<ActionBase>();

    [Header("Environment Variables")] 
    [SerializeField] private Dictionary<Vector2Int, Node> Grid;
    [SerializeField] private Node CurrentNode;
    
    private Animator EntityAnimtor;
    private GridManager _gridManager;
    private PathFinding _pathFinding;

    public virtual void InitActions(ActionBase action, float ActionRange)
    {
        action.Init(gameObject, ActionRange, _gridManager);
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
    
    public float GetHealth => Health;
    public float GetDamage => Damage;
    public float GetDefence => Defence;
    public float GetMoveSpeed => MoveSpeed;
    public Animator GetAnimator => EntityAnimtor;
    public List<ActionBase> GetAbilityList => Abilities;
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
        
        //Action specfic initialisation
        InitActions(new MoveAction(), MoveSpeed);
    }
}
