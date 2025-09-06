using System.Collections.Generic;
using UnityEngine;

namespace src.New_Testing_Scripts.TileMapTesting
{
    public class NewPathfinder 
    {
        public Dictionary<Vector2Int , NewNode> Grid = new Dictionary<Vector2Int , NewNode>();
        public Vector2 _gridSize;
    
        private static readonly Vector2Int[] Directions = {
            new Vector2Int(1, 0),   // right-down visually
            new Vector2Int(-1, 0),  // left-up visually
            new Vector2Int(0, 1),   // left-down visually
            new Vector2Int(0, -1)   // right-up visually
        };

        public NewPathfinder(Dictionary<Vector2Int, NewNode> grid)
        {
            this.Grid = grid;
        }


        #region Public Methods
        public Dictionary<Vector2Int , NewNode> GetGrid =>  Grid;
        
        
        public void SetGrid(Dictionary<Vector2Int , NewNode> grid)
        {
            Grid = grid;
        }
        
        
        public List<NewNode> GetPath(NewNode startNode, NewNode goalNode)
        {
            if (startNode == null || goalNode == null)
            {
                Debug.LogError("GetPath: start or goal is null.");
                return null;
            }
            Queue<NewNode> OpenSet = new Queue<NewNode>(); 
            Queue<NewNode> ClosedSet = new Queue<NewNode>();
            
            OpenSet.Enqueue(startNode);
            startNode.SetWalkableState(true);
            startNode.GCost = 0;
            startNode.HCost = Get_HCost(startNode, goalNode);

            while (OpenSet.Count > 0)
            {
                NewNode currentNode = OpenSet.Dequeue();
                ClosedSet.Enqueue(currentNode);
                
                if(currentNode == goalNode)
                    return ReconstructPath(startNode, goalNode);

                foreach (NewNode neighbour in GetNeighbours(currentNode))
                {
                    if(!neighbour.CanNavigate || ClosedSet.Contains(neighbour))                
                        continue;   
                    
                    float tentativeGCost = currentNode.GCost + Get_GCost(startNode, neighbour);
                    if(!OpenSet.Contains(neighbour) || tentativeGCost < neighbour.GCost)
                    {
                        neighbour.Parent = currentNode;
                        neighbour.GCost = tentativeGCost;
                        neighbour.HCost = Get_HCost(neighbour, goalNode);
                        
                        if(!OpenSet.Contains(neighbour))
                            OpenSet.Enqueue(neighbour);
                    }
                }
            }
            Debug.LogWarning("No path found.");
            return null;
        }
    

        #endregion

        #region private helper methods
 

        private List<NewNode> GetNeighbours(NewNode node)
        {
            List<NewNode> neighbours = new List<NewNode>();
            foreach (var dir in Directions)
            {
                Vector2Int checkPos = node.GetGridPosition + dir;
                if (Grid.ContainsKey(checkPos))
                    neighbours.Add(Grid[checkPos]);
            }
            return neighbours;
        }

        private List<NewNode> ReconstructPath(NewNode startNode, NewNode goalNode)
        {
            List<NewNode> path = new List<NewNode>();
            NewNode currentNode = goalNode;
            
            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }
            path.Reverse();
            return path;
        }

        private float Get_GCost(NewNode currentNode, NewNode nextNode)
        {
            //return Mathf.Abs(Vector2.Distance(startNode.GetGridPosition, currentNode.GetGridPosition));
            return 1f;
        }

        private float Get_HCost(NewNode currentNode, NewNode goalNode)
        {
            return Mathf.Abs(currentNode.GetGridPosition.x - goalNode.GetGridPosition.x) + 
                   Mathf.Abs(currentNode.GetGridPosition.y - goalNode.GetGridPosition.y);
        }
        #endregion
    }
}
