using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : ActionBase
{
    private PathFinding PathFinder;
    private Dictionary<Vector2Int, Node> Grid_Nodes;
    private Node TargetNode;

    public void SetVariables(PathFinding _pathFinding, Node TargetNode, Dictionary<Vector2Int, Node> Grid)
    {
        this.PathFinder = _pathFinding;
        this.TargetNode = TargetNode;
        this.Grid_Nodes = Grid;
    }
    
    public IEnumerator Action(GameObject targert)
    {
        Vector3 CurrentUnitPosition = ParentObject.transform.position, TargetPos = TargetNode.GetRealPosition;

        Vector2Int currentGridPosition = new Vector2Int(Mathf.RoundToInt(CurrentUnitPosition.x),
            Mathf.RoundToInt(CurrentUnitPosition.y));
            
        Vector2Int goalPositionGrid = new Vector2Int(Mathf.RoundToInt(TargetPos.x),
            Mathf.RoundToInt(TargetPos.y));

        List<Node> path = PathFinder.GetPath(Grid_Nodes[currentGridPosition], Grid_Nodes[goalPositionGrid]);

        if (path == null || path.Count <= 0)
        {
            Debug.LogError("Path is null or empty!");
            yield break;
        }
            
        foreach (var node in path)
            yield return Move(node);
    }

    private IEnumerator Move(Node TargetNode)
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

