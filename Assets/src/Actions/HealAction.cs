using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAction : ActionBase
{

    /*
     * 
     * Create an empty C# script with ActionBase as it's parent Class. Look at how I implemented the ActionBase and MoveAction class respectively and 
     * Implement a 'Heal' that will add an amount (float) to a target. You will have to access the BaseBattleEntity component of the target, 
     * check the target's 'UnitOwner' variable to check if it's owned by 'Player' and call the 'Heal' on target. 
     * 
     * Lets say Action Method basically checks the current node the player is in and checks the node the player needs to be healed in.
     * After it checks that, it checks whether or not it is within range. if that is true, it does that action. If not, Log error tells player that it doesnt work
     *
     *
     * 
     * 
     * 
     */

    private float healthpoints = 3f;

    public override IEnumerator Action(GameObject target)
    {
     


        Heal(target);
        yield return base.Action(target);

    }

    public void Heal(GameObject target) 
    {
        BaseBattleEntity entity = target.GetComponent<BaseBattleEntity>();

        if (entity.GetUnitOwnerShip == UnitOwnership.Player || entity.GetUnitOwnerShip == UnitOwnership.Ally)
        {
            entity.Heal(healthpoints);
        }
    }


}
