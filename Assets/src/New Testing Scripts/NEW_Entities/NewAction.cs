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

            NewNode startOfPath = gridSystem.GetNodeAtTarget(ActionOwner.gameObject);
            NewNode endOfPath = gridSystem.GetNodeAtTarget(target.gameObject);
            
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
                    yield return Move(ActionOwner.gameObject.transform.position, node);
                    path[currentNodeIndex].SetWalkableState(true);
                    currentNodeIndex++;
                }
                else
                    break;
            }        
        }
        
        private IEnumerator Move(Vector3 startPosition, NewNode TargetNode)
        {
            Transform entityTransform = ActionOwner.gameObject.transform;
            float duration = 0.15f, elapsedTIme = 0;
            Vector3 StartPosition = startPosition,
                EndPosition = TargetNode.GetRealPosition;
        
            while (elapsedTIme<duration)
            {
                entityTransform.position = Vector3.Lerp(StartPosition, EndPosition, elapsedTIme / duration);
                elapsedTIme += Time.deltaTime;
                yield return null;
            }
            ActionOwner.GetMovementPoints--;
            ActionOwner.GridPosition = new Vector2Int(Mathf.FloorToInt(EndPosition.x), Mathf.FloorToInt(EndPosition.y));
            entityTransform.position = EndPosition;
        }
    }
}
