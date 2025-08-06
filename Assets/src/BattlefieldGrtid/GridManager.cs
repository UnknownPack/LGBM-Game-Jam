using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab;
    [SerializeField] private int GridRadius = 15;
    private Dictionary<Vector2Int , Node> Grid_Nodes = new Dictionary<Vector2Int , Node>();
    
    
    private PathFinding pathFinding = new PathFinding();

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


    [ContextMenu("Test Grid Nodes")]
    public void TestGridNodeFunctinality()
    {
        if (parentNode != null)
        {
            DestroyImmediate(parentNode);
            parentNode = null;  
        } 
        GenerateMap();
        TestPathfinding();
    }
    
    private void GenerateMap()
    { 
        Grid_Nodes = new Dictionary<Vector2Int , Node>(); 
        parentNode =  new GameObject(" --- Nodes --- ");
        int xCord = 0;
        for (int i = -GridRadius; i <= GridRadius; i++)
        {
            var yCord = 0;
            for (int p = GridRadius; p >= -GridRadius; p--)
            {
                Vector3 RealPosition = new Vector3(i, p);
                Vector2Int GridPostion = new Vector2Int(xCord, yCord);
                GameObject tile = Instantiate(tilePrefab, parentNode.transform, true);
                tile.transform.position = new Vector3(i, p, 10);
                Grid_Nodes[GridPostion] = new Node(GridPostion, RealPosition, tile, true);
                yCord++;
                Debug.Log($"Tile's Real position: {RealPosition} \n Grid Position: {GridPostion}");
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

    public PathFinding GetPathFinding => pathFinding;

    public Dictionary<Vector2Int, Node> GetGridNodes => Grid_Nodes;

    #endregion
    
    #region Testing Functions

    
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
