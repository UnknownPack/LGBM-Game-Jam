using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackAction : ActionBase
{
    //default amount of damage
    private float damageAmount = 1f;
    private UnitOwnership targetOwner = UnitOwnership.Enemy;

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
        
        PlayAbilityAnimation("Attack");
        entity.TakeDamage(damageAmount);

        yield return base.Action(target);
    }
}

