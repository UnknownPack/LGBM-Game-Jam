using UnityEngine;

public class DamageGuy : BaseBattleEntity
{
    [Header("\nDamage Stats")]
    public float AttackRange = 1f;
    public float grenadeRange = 2f;
    public float DamageAmount = 1f;
    protected override void InitialiseActions()
    {
        base.InitialiseActions();
        MeleeAttackAction meleeAttackAction = new MeleeAttackAction();
        meleeAttackAction.SetDamageAmount(DamageAmount);
        meleeAttackAction.SetTargetType(UnitOwnership.Enemy);
        InitActions(AbilityName.Attack, meleeAttackAction, AttackRange, ActionPoint_Cost, ActionType.ActionPoint,
            ActionTargetType.Unit);
        
        InitActions(AbilityName.Grenade, new GrenadeAction(), grenadeRange, ActionPoint_Cost, ActionType.ActionPoint, ActionTargetType.Tile);
    }
}
