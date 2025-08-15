using System.Collections;
using System.Collections.Generic;
using System.Linq; 
using UnityEngine;

public class EnemyBaseEntity : BaseBattleEntity
{
    [Header("\nDamage Stats")]
    public float AttackRange = 1f;
    public virtual IEnumerator ExecuteTurn()
    {
        Debug.Log($"{gameObject.name}'s Turn is being executed!");
        BaseBattleEntity TargetUnit = SelectNearestTarget();
        List<ActionBase> Plan = GeneratePlan(TargetUnit.gameObject);
        
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
        
        foreach (var action in Plan)
        {
            Debug.Log($"{action.GetType().Name}");
            if(TargetUnit == null || _gridManager.noPathToTarget(TargetUnit.transform.position))
            {
                TargetUnit = SelectNearestTarget();
                if(TargetUnit == null|| _gridManager.noPathToTarget(TargetUnit.transform.position))
                {
                    TargetUnit = SelectNearestTarget();
                    if(TargetUnit == null|| _gridManager.noPathToTarget(TargetUnit.transform.position))
                    {
                        Debug.LogError("Target unit is null, cannot execute action!, Skipping turn!");
                        continue;
                    } 
                } 
            }
            yield return StartCoroutine(action.Action(TargetUnit.gameObject));
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
        float distanceToTarget = Vector2Int.Distance(_gridManager.GetNodeFromPosition(transform.position).GetGridPosition
            , _gridManager.GetNodeFromPosition(target.transform.position).GetGridPosition);
        Debug.Log($"Distance to target: {distanceToTarget}!, current move speed: {GetMoveSpeed}, current attack range: {AttackRange}");
        
        if (distanceToTarget <= AttackRange)
        {
            plan.Add(GetAbilityList[AbilityName.Attack]);
        }
        else if (distanceToTarget <= GetMoveSpeed + AttackRange)
        {
            plan.Add(GetAbilityList[AbilityName.Move]);
            plan.Add(GetAbilityList[AbilityName.Attack]);
        }
        else
        {
            plan.Add(GetAbilityList[AbilityName.Move]);
        }
        return plan;
    }
    
    protected virtual BaseBattleEntity SelectNearestTarget(BaseBattleEntity excusionTarget = null)
    {
        /*
         * Implement logic that will select a player unit
         */
        List<BaseBattleEntity> filteredList = _gridManager.GetBattleEntitiesList().
            Where(obj => obj.GetUnitOwnerShip == UnitOwnership.Player).ToList();

        BaseBattleEntity closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (BaseBattleEntity target in filteredList)
        {
            if(target == excusionTarget)
                continue;
            
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = target;
            }
        }
        Debug.LogWarning($"Checking target: {closestTarget.gameObject.name}, {_gridManager.GetNodeFromPosition(closestTarget.transform.position).GetGridPosition}");
        return closestTarget;
    }

    protected override void InitialiseActions()
    {
        // Initialise the actions for the enemy entity
        InitActions(AbilityName.Move, new EnenyMoveAction(), GetMoveSpeed, MovePoint_Cost, ActionType.MovePoint, ActionTargetType.Tile);
        MeleeAttackAction meleeAttackAction = new MeleeAttackAction();
        meleeAttackAction.SetDamageAmount(GetDamage());
        meleeAttackAction.SetTargetType(UnitOwnership.Player);
        InitActions(AbilityName.Attack, meleeAttackAction, AttackRange, ActionPoint_Cost, ActionType.ActionPoint,
            ActionTargetType.Unit);
    }
}
