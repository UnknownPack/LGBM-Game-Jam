using System.Collections;
using UnityEngine;

public class DefenceBuffAction : ActionBase
{
    public void SetDuration(int duration) => this.duration = duration;
    private int duration;
    public override IEnumerator Action(GameObject target)
    {
        DefenceBoost defenceBoost = new DefenceBoost();
        defenceBoost.Init(turnManager, ParentObject , duration);
        
        Debug.Log("Self Dence boost!");
        //TODO: TRIGGER ANIMATION(S) HERE
        
        // Remove action points etc.
        yield return base.Action(target);
    }
    
    public override ActionTargetType GetActionTargetType => ActionTargetType.None;
}
