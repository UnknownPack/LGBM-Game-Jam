using System.Collections;
using System.Collections.Generic;
using src.New_Testing_Scripts.TileMapTesting;
using Unity.VisualScripting;
using UnityEngine;

namespace src.New_Testing_Scripts.NEW_Entities
{
    public abstract class NewAction
    {
        protected NewEntityBase ActionOwner;

        public NewAction(NewEntityBase actionOwner)
        {
            ActionOwner = actionOwner;
        }


        public abstract IEnumerator Action(GameObject target);
    }

    public class MoveAction : NewAction
    {
        public MoveAction(NewEntityBase entity) : base(entity)
        {
            
        }

        public override IEnumerator Action(GameObject target)
        {
            GridSystem gridSystem = ServiceLocator.Get<GridSystem>();
            if (gridSystem == null)
            {
                Debug.LogError("Service Locator did not return Grid System");
                yield break;
            }

            NewNode startOfPath = gridSystem.GetNodeAtWorld(ActionOwner.gameObject.transform.position);
            NewNode endOfPath = gridSystem.GetNodeAtWorld(target.gameObject.transform.position);
            
            List<NewNode> path = gridSystem.GetPath(startOfPath, endOfPath);
            
            if (path == null)
            {
                Debug.LogError("Path is null!");
                yield break;
            }

            if (path.Count <= 0)
            {
                Debug.LogError("Path is empty!");
                yield break;
            }

            if (path.Count > ActionOwner.GetMovementPoints)
            {
                Debug.LogError("Path is greater than move range \n cancelling action!");
                yield break;
            }
            
            int currentNodeIndex = 0;
            foreach (var node in path)
            {
                if (node.CanNavigate)
                {
                    yield return Move(ActionOwner.gameObject.transform.position, node, gridSystem);
                    path[currentNodeIndex].SetWalkableState(true);
                    currentNodeIndex++;
                }
                else
                    break;
            }        
            yield break;
        }
        
        // ✅ FIX: updated to use Tilemap.GetCellCenterWorld
        private IEnumerator Move(Vector3 startPosition, NewNode TargetNode, GridSystem gridSystem)
        {
            Transform entityTransform = ActionOwner.gameObject.transform;
            float duration = 0.15f, elapsedTime = 0;
            Vector3 StartPosition = startPosition;

            // ✅ FIX: use tile center, not TargetNode.GetRealPosition
            Vector3 EndPosition = gridSystem.Tilemap.GetCellCenterWorld(
                new Vector3Int(TargetNode.GetGridPosition.x, TargetNode.GetGridPosition.y, 0)
            );

            while (elapsedTime < duration)
            {
                entityTransform.position = Vector3.Lerp(StartPosition, EndPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            ActionOwner.GetMovementPoints--;
            ActionOwner.GridPosition = TargetNode.GetGridPosition; // ✅ FIX: set logical grid position directly
            entityTransform.position = EndPosition; // ✅ FIX: snap to cell center
        }
    }
}
