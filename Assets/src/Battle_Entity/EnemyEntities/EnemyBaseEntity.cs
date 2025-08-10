using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class EnemyBaseEntity : BaseBattleEntity
{
    

    [Header("\nDamage Stats")]
    public float AttackRange = 1f;
    public float DamageAmount = 1f;


    public virtual IEnumerator ExecuteTurn()
    {
        Debug.Log($"{gameObject.name}'s Turn is being executed!");
        GameObject TargetUnit = SelectNearestTarget();
        List<ActionBase> Plan = GeneratePlan(TargetUnit);
        
        if(TargetUnit == null)
        {
            Debug.LogError("No valid target selected/ found!");
            yield break;
        }

        if (Plan == null || Plan.Count <= 0)
        {
            Debug.LogError("Plan is null or empty!");
            yield break;
        }
        
        foreach (var action in GeneratePlan(TargetUnit))
        {
            yield return StartCoroutine(action.Action(TargetUnit));
        }
        Debug.Log($"{gameObject.name}'s turn is finished!");
    }

    protected virtual List<ActionBase> GeneratePlan(GameObject target)
    {
        /* Implement logic that will generate a plan
         * For example:
         *  Create an algorithm that will decide either to move to
         *  or attack player. If enemy si not within range, simply try
         *  to get as close as possible to player by adding the move action
         *  to the plan. If enemy within range add the 'attack' action to this Plan
         *
         *  if the enemy can do both in one turn, add both actions appropriately
         */


        //just doing a default (will attack nearest player)
        List<ActionBase> plan = new List<ActionBase>();

        if (GetAbilityList.TryGetValue(AbilityName.Move, out ActionBase moveAction))
        {
            moveAction.TargetWithinRange(target);
            //plan.Add(moveAction);
        }

        return plan;
    }
    
    protected virtual GameObject SelectNearestTarget()
    {
        /*
         * Implement logic that will select a player unit
         */

        GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");
        List<GameObject> playerUnits = new List<GameObject>();

        foreach (GameObject unit in units)
        {
            BaseBattleEntity entity = unit.GetComponent<BaseBattleEntity>();
            if (entity != null && entity.GetUnitOwnerShip == UnitOwnership.Player)
            {
                playerUnits.Add(unit);
            }
        }

        GameObject closestTarget;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject target in playerUnits)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                return target;
            }
        }

        return null;
    }

    protected override void InitialiseActions()
    {
        base.InitialiseActions();
        //Intialise Enemy Specific Actions here
        MeleeAttackAction meleeAttackAction = new MeleeAttackAction();
        meleeAttackAction.SetDamageAmount(DamageAmount);
        InitActions(AbilityName.Attack, meleeAttackAction, AttackRange, ActionPoint_Cost, ActionType.ActionPoint,
            ActionTargetType.Unit);
    }
}
