using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiseBaricadeAction : ActionBase
{
    public void SetBaricadeRadius(float radius) => BaricadeRadius = radius;
    private float BaricadeRadius = 1;
    
    public override IEnumerator Action(GameObject target)
    {
        List<Node> NodesWithinAoe = GetNodesWithinAoe(target, BaricadeRadius);
        foreach (var node in NodesWithinAoe)
        {
            node.SetWalkableState(false);
            node.GetTileObject.GetComponent<SpriteRenderer>().color = Color.red; // Change color to indicate it's blocked
        }
        
        Debug.Log("Baricades raised in the area!");
        //TODO: TRIGGER ANIMATION(S) HERE
        PlayAbilityAnimation("Heal");

        // Remove action points etc.
        yield return base.Action(target);
    }
    
    public override ActionTargetType GetActionTargetType => ActionTargetType.Tile;
}
