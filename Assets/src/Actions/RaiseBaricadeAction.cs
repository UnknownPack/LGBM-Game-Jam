using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiseBaricadeAction : ActionBase
{
    public void SetBaricadeRadius(float radius) => BaricadeRadius = radius;
    private float BaricadeRadius = 2;
    
    public override IEnumerator Action(GameObject target)
    {
        List<Node> NodesWithinAoe = GetNodesWithinAoe(target, BaricadeRadius);
        foreach (var node in NodesWithinAoe)
            node.SetWalkableState(false);
        
        Debug.Log("Baricades raised in the area!");
        //TODO: TRIGGER ANIMATION(S) HERE
        
        // Remove action points etc.
        yield return base.Action(target);
    }
}
