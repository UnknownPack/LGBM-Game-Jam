using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : ActionBase
{
    //dictates the speed a unit will move fdrom tile to tile
    private float moveSpeed = 0.15f ;
    public override IEnumerator Action(GameObject target)
    {
        Node CurrentNode = _gridManager.GetNodeFromPosition(ParentObject.transform.position);
        Node TargetNode = _gridManager.GetNodeFromPosition(target.transform.position); 

        List<Node> path = PathFinder.GetPath(CurrentNode, TargetNode);
        
        if (path == null || path.Count <= 0)
        {
            Debug.Log($"Start Node Position: {CurrentNode.GetGridPosition} \n Goal Node Position: {TargetNode.GetGridPosition}");
            Debug.LogError("Path is null or empty!");
            yield break;
        }
            
        foreach (var node in path)
            yield return Move(ParentObject.transform.position, node);
        
        yield return base.Action(target);
    }

    private IEnumerator Move(Vector3 startPosition, Node TargetNode)
    { 
        float duration = moveSpeed, elapsedTIme = 0;
        Vector3 StartPosition = startPosition,
            EndPosition = TargetNode.GetRealPosition;
        
        while (elapsedTIme<duration)
        {
            ParentObject.transform.position = Vector3.Lerp(StartPosition, EndPosition, elapsedTIme / duration);
            elapsedTIme += Time.deltaTime;
            yield return null;
        }
        ParentObject.transform.position = EndPosition;
        TargetNode.SetWalkableState(false);
    }
}

