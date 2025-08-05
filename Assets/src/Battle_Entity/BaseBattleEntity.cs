using System.Collections.Generic;
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

    private Animator EntityAnimtor;

    public virtual void InitActions(ActionBase action)
    {
        action.Init(gameObject);
        Abilities.Add(action);
    }
    
    
    void Start()
    {
        EntityAnimtor = GetComponent<Animator>();
    }
    
    

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public float GetHealth => Health;
    public float GetDamage => Damage;
    public float GetDefence => Defence;
    public float GetMoveSpeed => MoveSpeed;
    public Animator GetAnimator => EntityAnimtor;
    public List<ActionBase> GetAbilityList => Abilities;
}
