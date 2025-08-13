using UnityEngine;

public class DamageGuy : BaseBattleEntity
{
    [Header("\nDamage Stats")]
    public float AttackRange = 1f;
    public float grenadeRange = 2f;
    public int insultDuration = 2;
    public GameObject grenadePrefab;

    private void Awake()
    {
        SetGrenadePrefab(grenadePrefab);
    }
    protected override void InitialiseActions()
    {
        base.InitialiseActions();
        
        MeleeAttackAction meleeAttackAction = new MeleeAttackAction();
        meleeAttackAction.SetDamageAmount(Damage);
        meleeAttackAction.SetTargetType(UnitOwnership.Enemy);
        InitActions(AbilityName.Attack, meleeAttackAction, AttackRange, ActionPoint_Cost, ActionType.ActionPoint,
            ActionTargetType.Unit);
        
        InitActions(AbilityName.Grenade, new GrenadeAction(), grenadeRange, ActionPoint_Cost, ActionType.ActionPoint, ActionTargetType.Tile);
        
        InsultAction insultAction = new InsultAction();
        insultAction.SetInsultDuration(insultDuration);
        InitActions(AbilityName.Insult, insultAction, grenadeRange, ActionPoint_Cost, ActionType.ActionPoint, ActionTargetType.Unit);
    }

}
