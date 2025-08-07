using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiseBaricadeAction : ActionBase
{
    public override IEnumerator Action(GameObject target)
    {
        Node TargetNode = _gridManager.GetNodeFromPosition(target.transform.position);
        List<Node> AreaOfEffect = GetListOfNodesAffected();

        foreach (var node in AreaOfEffect)
        {
            node.SetWalkableState(false);
            SpriteRenderer sr = node.GetTileObject.GetComponent<SpriteRenderer>();
            if(sr == null)
                yield break;
            
            sr.color = Color.red;
            //TODO: IMplement future logic to reverse this after a certain amount of turns
        }
    }
}
