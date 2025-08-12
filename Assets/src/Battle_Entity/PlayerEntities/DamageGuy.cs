using UnityEngine;

public class DamageGuy : BaseBattleEntity
{
    [Header("\nDamage Stats")]
    public float AttackRange = 1f;
    public float grenadeRange = 2f;
    public float DamageAmount = 1f;
    public int insultDuration = 2;
    protected override void InitialiseActions()
    {
        base.InitialiseActions();
        
        MeleeAttackAction meleeAttackAction = new MeleeAttackAction();
        meleeAttackAction.SetDamageAmount(DamageAmount);
        meleeAttackAction.SetTargetType(UnitOwnership.Enemy);
        InitActions(AbilityName.Attack, meleeAttackAction, AttackRange, ActionPoint_Cost, ActionType.ActionPoint,
            ActionTargetType.Unit);
        
        InitActions(AbilityName.Grenade, new GrenadeAction(), grenadeRange, ActionPoint_Cost, ActionType.ActionPoint, ActionTargetType.Tile);
        
        InsultAction insultAction = new InsultAction();
        insultAction.SetInsultDuration(insultDuration);
        InitActions(AbilityName.Insult, insultAction, grenadeRange, ActionPoint_Cost, ActionType.ActionPoint, ActionTargetType.Unit);
    }
}
