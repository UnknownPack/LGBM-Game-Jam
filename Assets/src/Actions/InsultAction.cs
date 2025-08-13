using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsultAction : ActionBase
{
    public override IEnumerator Action(GameObject target)
    {
        BaseBattleEntity entity = target.GetComponent<BaseBattleEntity>();
        if (entity == null)
        {
            Debug.LogError("Target does not have a BaseBattleEntity component.");
            yield break;
        }

        PlayAbilityAnimation("Insult");

        //put functionality here
        //entity.Insult();

        yield return base.Action(target);
    }
}
