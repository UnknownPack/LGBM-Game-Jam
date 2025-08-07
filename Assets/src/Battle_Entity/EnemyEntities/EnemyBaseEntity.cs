using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseEntity : BaseBattleEntity
{
    public virtual IEnumerator ExecuteTurn()
    {
        Debug.Log($"{gameObject.name}'s Turn has been executed!");
        yield return null;
    }

    protected override void InitialiseActions()
    {
        base.InitialiseActions();
        //Intialise Enemy Specific Actions here
    }
}
