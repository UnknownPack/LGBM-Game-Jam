using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : ActionBase
{
    private PathFinding _pathFinding;

    public override void Init(GameObject parentObject)
    {
        base.Init(parentObject);
        parentObject.
    }
    
    public IEnumerator Action(Node EndPosition)
    {
        Vector3 worldPos = ParentObject.transform.position;
        Vector3 TargetPos = EndPosition.realPosition;
            
            new Vector2Int(Random.Range(0, 7), 0);
        Vector2Int goalPosition = new Vector2Int(Random.Range(0, 7), Random.Range(1, 7));
        
        List<Node> path = pathFinding.GetPath(Grid_Nodes[startPosition], Grid_Nodes[goalPosition]);
        if (path != null)
        { 
            foreach (var node in path)
            { 
                if (node.GetGridPosition == goalPosition)
                    Debug.LogWarning($"Reached Goal Node: {node.GetGridPosition}!");
            }
        }
        
        else
            Debug.LogWarning("No path found.");
    }
}
