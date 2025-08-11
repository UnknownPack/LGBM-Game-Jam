using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class EnenyMoveAction : MoveAction
{
   protected override List<Node> CreatePath(GameObject target)
   {  
      Vector3 currentPos = ParentObject.transform.position, targetPos = target.transform.position;
      Node CurrentNode = _gridManager.GetNodeFromPosition(currentPos);
      bool isTargetWithinRange = Vector2Int.Distance(CurrentNode.GetGridPosition, _gridManager.GetNodeFromPosition(targetPos).GetGridPosition) <= ActionRange;

      
      Node TargetNode = isTargetWithinRange
         ? _gridManager.GetNodeFromPosition(targetPos)
         : _gridManager.FindClosestNodeToTarget(_gridManager.GetNodeFromPosition(targetPos).GetGridPosition , ActionRange);
      
      if(TargetNode == null)
      {
         Debug.LogError("Target node not found");
         return null;
      }
      
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
      
      path.Remove(TargetNode);
      return path;
   }
   
   
}
