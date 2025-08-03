using System.Collections.Generic;
using UnityEngine;

public class PathFinding
{
        public Dictionary<Vector2, Node> Grid = new Dictionary<Vector2, Node>();
    public Vector2 _gridSize;


    #region Public Methods
        public Dictionary<Vector2, Node> GetGrid()
        {
            return Grid;
        }
        
        public void SetGrid(Dictionary<Vector2, Node> grid)
        {
            Grid = grid;
        }
        
        
        public List<Node> GetPath(Node startNode, Node goalNode)
        {
            Queue<Node> OpenSet = new Queue<Node>(); 
            Queue<Node> ClosedSet = new Queue<Node>();
            
            OpenSet.Enqueue(startNode);
            startNode.GCost = 0;
            startNode.HCost = Get_HCost(startNode, goalNode);

            while (OpenSet.Count > 0)
            {
                Node currentNode = OpenSet.Dequeue();
                ClosedSet.Enqueue(currentNode);
                
                if(currentNode == goalNode)
                    return ReconstructPath(startNode, goalNode);

                foreach (Node neighbour in GetNeighbours(currentNode))
                {
                    if(!neighbour._canNavigateTo || ClosedSet.Contains(neighbour))                
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
        
        public (Node, Node) TranslatePosition_ToNodes(Vector2 StartPostion, Vector2 GoalPosition)
        {
            Node startNode = Grid.TryGetValue(StartPostion, out var value1) ? value1 : null;
            Node goalNode = Grid.TryGetValue(GoalPosition, out var value2) ? value2 : null;

            if (startNode == null || goalNode == null)
            {
                Debug.LogError("Start or Goal node not found in the grid.");
                return (null, null);
            }
            
            return (startNode, goalNode);
        }
    

    #endregion

    #region private helper methods
        private List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    neighbours.Add(Grid[node._position]);
                }
            }
            return neighbours;
        }

        private List<Node> ReconstructPath(Node startNode, Node goalNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = goalNode;
            
            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }
            path.Reverse();
            return path;
        }

        private float Get_GCost(Node startNode, Node currentNode)
        {
            return Mathf.Abs(Vector2.Distance(startNode._position, currentNode._position));
        }

        private float Get_HCost(Node currentNode, Node goalNode)
        {
            return Mathf.Abs(currentNode._position.x - goalNode._position.x) + 
                   Mathf.Abs(currentNode._position.y - goalNode._position.y);
        }
    #endregion


     
}
