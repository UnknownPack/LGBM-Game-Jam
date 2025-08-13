using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsultAction : ActionBase
{
    public void SetInsultDuration(int duration) => this.duration = duration;
    private int duration;
    
    public override IEnumerator Action(GameObject target)
    {
        Insult insult = new Insult();
        insult.Init(turnManager, target, duration);
        
        Debug.Log("Insulted Target!");

        PlayAbilityAnimation("Insult");
        // Remove action points etc.
        yield return base.Action(target);
    }
    
    public override ActionTargetType GetActionTargetType => ActionTargetType.Unit;
}
