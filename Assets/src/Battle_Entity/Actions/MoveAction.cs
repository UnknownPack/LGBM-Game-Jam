using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : ActionBase
{
    private PathFinding PathFinder;
    private Dictionary<Vector2Int, Node> Grid_Nodes;
    private GridManager _gridManager;

    public MoveAction(GridManager gridManager)
    {
        
        this._gridManager = gridManager;
        this.PathFinder = _gridManager.GetPathFinding;
        this.Grid_Nodes = _gridManager.GetGridNodes;
    }
    public override IEnumerator Action(GameObject targert)
    {
        Node CurrentNode = _gridManager.GetNodeFromPosition(ParentObject.transform.position);
        Node TargetNode = _gridManager.GetNodeFromPosition(targert.transform.position); 

        List<Node> path = PathFinder.GetPath(CurrentNode, TargetNode);
        
        if (path == null || path.Count <= 0)
        {
            Debug.Log($"Start Node Position: {CurrentNode.GetGridPosition} \n Goal Node Position: {TargetNode.GetGridPosition}");
            Debug.LogError("Path is null or empty!");
            yield break;
        }
            
        foreach (var node in path)
            yield return Move(ParentObject.transform.position, node);
    }

    private IEnumerator Move(Vector3 startPosition, Node TargetNode)
    { 
        float duration = 1f, elapsedTIme = 0;
        Vector3 StartPosition = ParentObject.transform.position,
            EndPosition = TargetNode.GetRealPosition;
        
        while (elapsedTIme<duration)
        {
            ParentObject.transform.position = Vector3.Lerp(StartPosition, EndPosition, elapsedTIme / duration);
            elapsedTIme += Time.deltaTime;
            yield return null;
        }
        ParentObject.transform.position = EndPosition;
    }
}

