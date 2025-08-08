using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAction : ActionBase
{
    private int blastRadius = 2;
    private int blastDamageMultiplier = 3;

    public override IEnumerator Action(GameObject target)
    {
        List<Node> NodesWithinAoe = GetNodesWithinAoe(target, blastRadius);
        Dictionary<Vector2Int, BaseBattleEntity> EntityGridPostions = _gridManager.GetEntityListToGrid();
        foreach (var node in NodesWithinAoe)
        {
            Vector2Int nodePosition = node.GetGridPosition;
            if(EntityGridPostions.ContainsKey(nodePosition))
                EntityGridPostions[nodePosition].TakeDamage(_baseBattleEntity.GetDamage * blastDamageMultiplier);
        }
        //TODO: TRIGGER ANIMATION(S) HERE
        
        // Remove action points etc.
        yield return base.Action(target);
    }
    
}
