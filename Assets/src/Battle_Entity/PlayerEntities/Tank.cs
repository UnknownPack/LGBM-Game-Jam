using UnityEngine;

public class Tank : BaseBattleEntity
{
    public float AttackRange = 1f;
    public int timesHit = 0;
    public int BuffDuration = 3;
    
    public override float GetDamage()
    { 
        float damage = Damage + (timesHit * 0.5f);
        timesHit = 0; 
        return damage;
    }
    protected override void InitialiseActions()
    {
        base.InitialiseActions();
        MeleeAttackAction meleeAttackAction = new MeleeAttackAction();
        meleeAttackAction.SetDamageAmount(GetDamage());
        meleeAttackAction.SetTargetType(UnitOwnership.Enemy);
        InitActions(AbilityName.Attack, meleeAttackAction, AttackRange, ActionPoint_Cost, ActionType.ActionPoint,
            ActionTargetType.Unit);
        
        DefenceBuffAction defenceBuffAction = new DefenceBuffAction();
        defenceBuffAction.SetDuration(BuffDuration);
        InitActions(AbilityName.DefenceBoost, defenceBuffAction, 0,ActionPoint_Cost, ActionType.ActionPoint, ActionTargetType.None);
    }

    public override void TakeDamage(float damageAmount)
    {
        timesHit ++;
        base.TakeDamage(damageAmount);
    }
}
