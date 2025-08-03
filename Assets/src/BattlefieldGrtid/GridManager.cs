using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab;
    [SerializeField] private int gridSize = 15;
    
    private new Dictionary<Vector2, Tile> map = new Dictionary<Vector2, Tile>();
    private new Dictionary<Vector2, GameObject> _grids = new Dictionary<Vector2, GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    [ContextMenu("Generate Nodes")]
    public void GenerateNodes()
    { 
        map = new Dictionary<Vector2, Tile>();
        _grids = new Dictionary<Vector2, GameObject>();
        GameObject parentNode =  new GameObject(" --- Nodes --- ");
        int x = 0, y = 0;
        for (int i = -gridSize; i <= gridSize; i++)
        {
            for (int p = -gridSize; p <= gridSize; p++)
            {
                Vector2 position = new Vector2(x, y);
                GameObject tile = Instantiate(tilePrefab, parentNode.transform, true);
                tile.transform.position = new Vector3(i, p, 10);
                _grids.Add(position, tile);
                map[position] = new Tile(tile, position);
                y++;
                Debug.Log($"Tile position: {x},{y}");
            }
            y = 0;
            x++; 
        }
        
    }
}
