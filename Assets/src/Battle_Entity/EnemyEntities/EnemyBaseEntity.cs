using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseEntity : BaseBattleEntity
{
    public virtual IEnumerator ExecuteTurn()
    {
        Debug.Log($"{gameObject.name}'s Turn is being executed!");
        GameObject TargetUnit = SelectTarget();
        List<ActionBase> Plan = GeneratePlan();
        
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
        
        foreach (var action in GeneratePlan())
        {
            yield return StartCoroutine(action.Action(TargetUnit));
        }
        Debug.Log($"{gameObject.name}'s turn is finished!");
    }

    protected virtual List<ActionBase> GeneratePlan()
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
        return null;
    }
    
    protected virtual GameObject SelectTarget()
    {
        /*
         * Implement logic that will select a player unit
         */
        return null;
    }

    protected override void InitialiseActions()
    {
        base.InitialiseActions();
        //Intialise Enemy Specific Actions here
    }
}
