using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackAction : ActionBase
{
    //default amount of damage
    protected float damageAmount = 1f;
    protected UnitOwnership targetOwner = UnitOwnership.Enemy;

    public void SetDamageAmount(float value) => damageAmount = value;
    public void SetTargetType(UnitOwnership owner) => targetOwner = owner;

    public override IEnumerator Action(GameObject target)
    {

        if (!target.CompareTag("Unit"))
        {
            Debug.LogError("Target doesn't have an Unit tag");
            yield break;
        }

        BaseBattleEntity entity = target.GetComponent<BaseBattleEntity>();

        if (entity.GetUnitOwnerShip != targetOwner)
            yield break;
        
        if (_baseBattleEntity is Tank tankEntity)
            tankEntity.SetHitTimes(0);
        
        if (_baseBattleEntity is DamageGuy damageGuyEntity)
        {
            if (entity.WillDie(_baseBattleEntity.GetDamage()))
            {
                damageGuyEntity.IncreaseGreedCounter(1);
                damageGuyEntity.GetActionPoints[ActionType.ActionPoint] = 1; 
            }
        }
        
        PlayAbilityAnimation("Attack");
        entity.TakeDamage(_baseBattleEntity.GetDamage());

        yield return base.Action(target);
    }
}

public class CollateralAttack : MeleeAttackAction
{
    private float damageTransferMultiplier = 0.65f;
    public override IEnumerator Action(GameObject target)
    {
        if (!target.CompareTag("Unit"))
        {
            Debug.LogError("Target doesn't have an Unit tag");
            yield break;
        }

        BaseBattleEntity entity = target.GetComponent<BaseBattleEntity>();

        if (entity.GetUnitOwnerShip != targetOwner)
            yield break;
        
        if (_baseBattleEntity is Tank tankEntity)
            tankEntity.SetHitTimes(0);
        
        PlayAbilityAnimation("Attack");
        entity.TakeDamage(_baseBattleEntity.GetDamage() * damageTransferMultiplier);
        
        BaseBattleEntity potentialTarget = _gridManager.GetCollateralEntityFromPosition(target, ParentObject);
        if (potentialTarget == null)
        {
            Debug.LogWarning($"Target not found in the direction of attack.");
            yield break;
        }
        potentialTarget.TakeDamage(_baseBattleEntity.GetDamage() * (1f - damageTransferMultiplier));
        potentialTarget.SetDefence(0);

        yield return base.Action(target);
    }
}

