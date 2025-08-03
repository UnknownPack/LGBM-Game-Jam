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
    
    public void GenerateMap()
    { 
        Grid_Nodes = new Dictionary<Vector2Int , Node>(); 
        parentNode =  new GameObject(" --- Nodes --- ");
        int xCord = 0;
        for (int i = -GridRadius; i <= GridRadius; i++)
        {
            var yCord = 0;
            for (int p = -GridRadius; p <= GridRadius; p++)
            {
                Vector2Int RealPosition = new Vector2Int(i, p);
                Vector2Int GridPostion = new Vector2Int(xCord, yCord);
                GameObject tile = Instantiate(tilePrefab, parentNode.transform, true);
                tile.transform.position = new Vector3(i, p, 10);
                Grid_Nodes[GridPostion] = new Node(GridPostion, RealPosition, tile, true);
                yCord++;
                Debug.Log($"Tile's Real position: {GridPostion} \n Grid Position: {RealPosition}");
            } 
            xCord++; 
        }
        pathFinding.SetGrid(Grid_Nodes);
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


}
