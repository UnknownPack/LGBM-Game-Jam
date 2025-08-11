using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab;
    [SerializeField, Range(2, 10)] private int GridXRadius;
    [SerializeField, Range(2, 10)] private int GridYRadius;
    private Dictionary<Vector2Int , Node> Grid_Nodes = new Dictionary<Vector2Int , Node>();
    private List<BaseBattleEntity> BattleEntitiesList = new List<BaseBattleEntity>();
    private PathFinding pathFinding = new PathFinding();
    private TurnManager turnManager;

    [SerializeField] private GameObject parentNode;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    
    public void InitaliseMap()
    {
        
        if (parentNode != null)
        {
            DestroyImmediate(parentNode);
            parentNode = null;  
        } 
        GenerateMap(); 
    }
    //
    //
    // [ContextMenu("Test Grid Nodes")]
    // public void TestGridNodeFunctinality()
    // {
    //     if (parentNode != null)
    //     {
    //         DestroyImmediate(parentNode);
    //         parentNode = null;  
    //     } 
    //     GenerateMap();
    //     TestPathfinding();
    // }
    //
    private void GenerateMap()
    { 
        Grid_Nodes = new Dictionary<Vector2Int , Node>(); 
        parentNode =  new GameObject(" --- Nodes --- ");
        int xCord = 0;
        for (int i = -GridXRadius; i <= GridXRadius; i++)
        {
            var yCord = 0;
            for (int p = GridYRadius; p >= -GridYRadius; p--)
            {
                Vector3 RealPosition = new Vector3(i, p);
                Vector2Int GridPostion = new Vector2Int(xCord, yCord);
                GameObject tile = Instantiate(tilePrefab, parentNode.transform, true);
                tile.transform.position = new Vector3(i, p, 10);
                Grid_Nodes[GridPostion] = new Node(GridPostion, RealPosition, tile, true);
                yCord++;
                //Debug.Log($"Tile's Real position: {RealPosition} \n Grid Position: {GridPostion}");
            } 
            xCord++; 
        }
        pathFinding.SetGrid(Grid_Nodes);
    }

    #region Public Helper Methods

    public List<Node> GetNodesWithinRadius(Vector3 origin, int radius)
    {
        List<Node> Output = new List<Node>();
        Vector2Int OriginGirdPosition = new Vector2Int(Mathf.RoundToInt(origin.x),
            Mathf.RoundToInt(origin.y));
        int xCord = OriginGirdPosition.x, yCord = OriginGirdPosition.y;
        for (int i = xCord - radius; i <= xCord + radius; i++)
        {
            for (int j = yCord - radius; j <= yCord + radius; j++)
            {
                Vector2Int vector = new Vector2Int(i, j);
                if(Grid_Nodes.TryGetValue(vector, out var node))
                    Output.Add(node);
            }
        }
        return Output;
    }

    public Node FindClosestNodeToTarget(Vector2Int targetPosition, float actionRange)
    {
        Debug.Log(targetPosition);

        Node start = Grid_Nodes[GetNodeFromPosition(transform.localPosition).GetGridPosition];
        if (start == null)
        {
            Debug.LogError($"FindClosestNodeToTarget: Start node is null at position {transform.position}.");
            return null;
        }

        Node end = Grid_Nodes[targetPosition];
        if (end == null)
        {
            Debug.LogError($"FindClosestNodeToTarget: End node is null at target position {targetPosition}.");
            return null;
        }
        Debug.Log($"FindClosestNodeToTarget: Start node {start.GetGridPosition}, End node {end.GetGridPosition}.");
        end.SetWalkableState(true);
        var path = pathFinding.GetPath(start, end);
        if (path == null)
        {
            Debug.LogError("FindClosestNodeToTarget: Path is null.");
            return null;
        }

        if (path.Count == 0)
        {
            Debug.LogError("FindClosestNodeToTarget: Path is empty.");
            return null;
        }

        // walk backward from the target until we're within actionRange of the target
        for (int i = path.Count - 1; i >= 0; i--)
        {
            if (Vector2Int.Distance(path[i].GetGridPosition, end.GetGridPosition) <= actionRange)
            {
                Debug.Log($"FindClosestNodeToTarget: Found valid node {path[i].GetGridPosition} within range {actionRange}.");
                return path[i];
            }
        }

        // fallback: first step on the path
        Debug.LogWarning("FindClosestNodeToTarget: No node found within actionRange, returning first path node.");
        path[path.Count].SetWalkableState(false);
        return path[0];
    }


    public Node GetNodeFromPosition(Vector3 position)
    {
        Node closestNode = null;
        float closestDistance = float.MaxValue;
        foreach (var node in Grid_Nodes)
        {
            float distance = Vector3.Distance(position, node.Value.GetRealPosition);
            if (distance <= closestDistance)
            {
                closestNode = node.Value;
                closestDistance = distance;
            }
        }
        return closestNode;
    }

    public void ResetColourTiles()
    {
        foreach (var node in Grid_Nodes)
        {
            SpriteRenderer spriteRenderer = node.Value.GetTileObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
                Debug.LogError("SpriteRender not found");
                
            spriteRenderer.color = Color.gray;
        }
    }

    public void AddEntityToGrid(BaseBattleEntity entity)
    {
        if (entity == null)
        {
            Debug.LogError("Entity is null, cannot add to grid.");
            return;
        }
        BattleEntitiesList.Add(entity); 
        GetNodeFromPosition(entity.transform.position).SetWalkableState(false);
    }
    public void RemoveEntityFromGrid(BaseBattleEntity entity)
    {
        if (entity == null)
        {
            Debug.LogError("Entity is null, cannot remove from grid.");
            return;
        }
        BattleEntitiesList.Remove(entity);
    }

    public Dictionary<Vector2Int, BaseBattleEntity> GetEntityListToGrid()
    {
        Dictionary<Vector2Int, BaseBattleEntity> entityGrid = new Dictionary<Vector2Int, BaseBattleEntity>();
        foreach (var entity in BattleEntitiesList)
        {
            Node node = GetNodeFromPosition(entity.transform.position);
            if (node != null)
            {
                entityGrid[node.GetGridPosition] = entity;
            }
            else
            {
                Debug.LogWarning($"Node not found for entity at position: {entity.transform.position}");
            }
        }
        return entityGrid;
    }

    public PathFinding GetPathFinding => pathFinding;
    public void InjectTurnManager(TurnManager turnManager) => this.turnManager = turnManager;
    public TurnManager GetTurnManager => turnManager;
    public void UpdateBattleList(List<BaseBattleEntity> entities) => BattleEntitiesList = entities;
    public List<BaseBattleEntity> GetBattleEntitiesList() => BattleEntitiesList;
    public Dictionary<Vector2Int, Node> GetGridNodes => Grid_Nodes;

    #endregion
    
    #region Testing Functions
    
    [ContextMenu("Regenerate Map")]
    public void Rebuild()
    {
        if (parentNode != null)
        {
            DestroyImmediate(parentNode);
            parentNode = null;  
        } 
        GenerateMap();
    }
    
    
    public void TestPathfinding()
    {
        Vector2Int startPosition = new Vector2Int(Random.Range(0, 7), 0);
        Vector2Int goalPosition = new Vector2Int(Random.Range(0, 7), Random.Range(1, 7));
        
        List<Node> path = pathFinding.GetPath(Grid_Nodes[startPosition], Grid_Nodes[goalPosition]);
        if (path != null)
        { 
            foreach (var node in path)
            { 
                if (node.GetGridPosition == goalPosition)
                    Debug.LogWarning($"Reached Goal Node: {node.GetGridPosition}!");
            }
        }
        
        else
            Debug.LogWarning("No path found.");
    }

    #endregion 


}
