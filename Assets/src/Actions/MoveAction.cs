using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : ActionBase
{
    //dictates the speed a unit will move fdrom tile to tile
    private float moveSpeed = 0.15f ;
    public override IEnumerator Action(GameObject target)
    {
        List<Node> path = CreatePath(target);
        if(path == null || path.Count == 0)
        {
            Debug.LogError("Path is null or empty!");
            yield break;
        }
        foreach (var node in path)
        {
            if (node.CanNavigate)
                yield return Move(ParentObject.transform.position, node);

            else
                break;
        }
        
        yield return base.Action(target);
    }
    
    protected virtual List<Node> CreatePath(GameObject target)
    {
        Node CurrentNode = _gridManager.GetNodeFromPosition(ParentObject.transform.position);
        Node TargetNode = _gridManager.GetNodeFromPosition(target.transform.position); 

        List<Node> path = PathFinder.GetPath(CurrentNode, TargetNode);
        
        if (path == null)
        {
            Debug.LogError("Path is null!");
            return null;
        }

        if (path.Count <= 0)
        {
            Debug.LogError("Path is empty!");
            return null;
        }
        
        return path;
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

