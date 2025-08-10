using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnenyMoveAction : MoveAction
{
   protected override List<Node> CreatePath(GameObject target)
   {
      Vector3 currentPos = ParentObject.transform.position, targetPos = target.transform.position;
      Node CurrentNode = _gridManager.GetNodeFromPosition(ParentObject.transform.position);
      bool isTargetWithinRange = Vector3.Distance(currentPos, targetPos) > ActionRange;
      
      Node TargetNode = isTargetWithinRange
         ? _gridManager.GetNodeFromPosition(targetPos)
         : _gridManager.FindClosestNodeToTarget(target, ActionRange);

      List<Node> path = PathFinder.GetPath(CurrentNode, TargetNode);
        
      if (path == null || path.Count <= 0)
      {
         Debug.Log($"Start Node Position: {CurrentNode.GetGridPosition} \n Goal Node Position: {TargetNode.GetGridPosition}");
         Debug.Log($"Distance from {CurrentNode.GetGridPosition} to {TargetNode.GetGridPosition} is {Vector2Int.Distance(CurrentNode.GetGridPosition, TargetNode.GetGridPosition)}");
         Debug.LogError("Path is null or empty!");
         return null;
      }
      
      if(isTargetWithinRange)
      {
         Debug.Log("Finding closest node to target!");
         Node lastNode = path[path.Count - 1];
         path.Remove(lastNode);
         
      }
      return path;
   }
   
   
}
