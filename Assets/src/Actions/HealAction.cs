using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAction : ActionBase
{

    public void SetHealthPoints(float value) => healthpoints = value;
    
    private float healthpoints = 3f;

    public override IEnumerator Action(GameObject target)
    {
        BaseBattleEntity entity = target.GetComponent<BaseBattleEntity>();
        if (entity == null)
        {
            Debug.LogError("Target does not have a BaseBattleEntity component.");
            yield break;
        }

        PlayAbilityAnimation("Heal");
        
        Debug.Log($"Healing: {target.name} for {healthpoints} points! Health is {entity.GetHealth} HP");
        entity.Heal(healthpoints);
        Debug.Log($"{target.name} healed! for {healthpoints} points! Health is {entity.GetHealth} HP");
        //TODO: TRIGGER ANIMATION(S) HERE
        yield return base.Action(target);
    }
    
    public override ActionTargetType GetActionTargetType => ActionTargetType.Unit;
    
}
