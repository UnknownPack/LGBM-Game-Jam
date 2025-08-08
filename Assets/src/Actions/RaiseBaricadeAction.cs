using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiseBaricadeAction : ActionBase
{
    public int BaricadeRadius = 2;
    
    public override IEnumerator Action(GameObject target)
    {
        List<Node> NodesWithinAoe = GetNodesWithinAoe(target, BaricadeRadius);
        foreach (var node in NodesWithinAoe)
        {
            node.SetWalkableState(false);
        }
        //TODO: TRIGGER ANIMATION(S) HERE
        
        // Remove action points etc.
        yield return base.Action(target);
    }
}
