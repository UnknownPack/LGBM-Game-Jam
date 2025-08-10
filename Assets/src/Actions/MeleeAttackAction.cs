using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackAction : ActionBase
{
    //default amount of damage
    [SerializeField] private float damageAmount = 1f;

    public void SetDamageAmount(float value) => damageAmount = value;

    public override IEnumerator Action(GameObject target)
    {

        if (!target.CompareTag("Unit"))
            yield break;

        BaseBattleEntity entity = target.GetComponent<BaseBattleEntity>();

        if (entity.GetUnitOwnerShip == UnitOwnership.Player)
            yield break;

        entity.TakeDamage(damageAmount);

        yield return base.Action(target);
    }
}

