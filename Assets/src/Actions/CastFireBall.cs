using System.Collections;
using UnityEngine;

public class CastFireBall : ActionBase
{
    public void SetDuration(int duration) => this.duration = duration;
    private int duration;
    public override IEnumerator Action(GameObject target)
    {
        FireMelt fireMelt = new FireMelt();
        fireMelt.Init(turnManager, target , duration);
        
        PlayAbilityAnimation("Heal");
        yield return base.Action(target);
    }
    
    public override ActionTargetType GetActionTargetType => ActionTargetType.Unit;
}
