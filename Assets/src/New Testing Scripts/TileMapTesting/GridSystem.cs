using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace src.New_Testing_Scripts.TileMapTesting
{
    public class GridSystem : MonoBehaviour
    {
        public GameObject tilePrefab;
        public Tilemap Tilemap;
        private NewPathfinder pathfinder;
        Dictionary<Vector2Int, NewNode> Grid;
        void Start()
        {
            GameObject previousParent = GameObject.Find("ParentObject");
            if (previousParent != null)
                DestroyImmediate(previousParent);
        
            Dictionary<Vector2Int, NewNode> grid = GenerateMap();
            Grid = grid;
            pathfinder = new NewPathfinder(grid);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                GameObject previousParent = GameObject.Find("ParentObject");
                if (previousParent != null)
                    DestroyImmediate(previousParent);

                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mouseWorld2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

                RaycastHit2D pointCheck = Physics2D.Raycast(mouseWorld2D, Vector2.zero);

                if (pointCheck.collider != null)  
                {
                    Vector3Int cellPos = Tilemap.WorldToCell(pointCheck.point);
                    Vector2Int gridPos = new Vector2Int(cellPos.x, cellPos.y);

                    if (Grid.ContainsKey(gridPos))
                    {
                        List<Vector2Int> keys = new List<Vector2Int>(pathfinder.GetGrid.Keys);
                        Vector2Int randomStartPos = keys[Random.Range(0, keys.Count)];

                        NewNode startNode = pathfinder.GetGrid[randomStartPos];
                        NewNode endNode = pathfinder.GetGrid[gridPos];

                        List<NewNode> path = pathfinder.GetPath(startNode, endNode);

                        PrintPath(path);
                    }
                    else
                        Debug.LogWarning("Clicked cell is outside of painted Tilemap!");
                }
                else
                    Debug.LogWarning("Click did not hit TilemapCollider2D");
            }
        }



        Dictionary<Vector2Int, NewNode> GenerateMap()
        {
            Dictionary<Vector2Int, NewNode> Grid = new Dictionary<Vector2Int, NewNode>();

            BoundsInt bounds = Tilemap.cellBounds;
            foreach (var pos in bounds.allPositionsWithin)
            {
                TileBase tile = Tilemap.GetTile(pos); 
            
                Vector2Int gridPosition = new Vector2Int(pos.x, pos.y);
            
                if (tile != null)
                {
                    Vector3 realPosition = Tilemap.CellToWorld(pos) ;
                    //Vector3 realPosition = Tilemap.CellToWorld(pos) + (Vector3)Tilemap.tileAnchor;
                    NewNode newNode = new NewNode(gridPosition, realPosition, tile, true);
                    Grid[gridPosition] = newNode;
                    Debug.Log($"Grid Position: {newNode.GetGridPosition}, Real Position: {newNode.GetRealPosition}, Can Navigate: {newNode.CanNavigate}");
                }
            }
            return Grid;
        }

        private void PrintPath(List<NewNode> path)
        {
            GameObject parentObject = new GameObject("ParentObject");
            foreach (var node in path)
            {
                var nodeMarker = Instantiate(tilePrefab, node.GetRealPosition, Quaternion.identity, this.transform);
                nodeMarker.transform.SetParent(parentObject.transform);
            }
        }
    
        [ContextMenu("Generate and print Grid")]
        public void GenerateAndPrintGrid()
        {
            Dictionary<Vector2Int, NewNode> grid = GenerateMap();
            pathfinder = new NewPathfinder(grid);
            GameObject parentObject = new GameObject("ParentObject");
            foreach (var node in pathfinder.GetGrid)
            {
                var nodeMarker = Instantiate(tilePrefab, node.Value.GetRealPosition, Quaternion.identity, this.transform);
                nodeMarker.transform.SetParent(parentObject.transform);
            }
        }
    
        [ContextMenu("Generate Path")]
        public void GenerateAndShowPath()
        {
            GameObject previousParent = GameObject.Find("ParentObject");
        
            if(previousParent!= null)
                DestroyImmediate(previousParent);
        
            Dictionary<Vector2Int, NewNode> grid = GenerateMap();
            pathfinder = new NewPathfinder(grid);
        
            List<Vector2Int> keys = new List<Vector2Int>(pathfinder.GetGrid.Keys);
            Vector2Int randomStartPos = keys[Random.Range(0, keys.Count)];
            Vector2Int randomEndPos = keys[Random.Range(0, keys.Count)];
        
            NewNode startNode = pathfinder.GetGrid[randomStartPos];
            NewNode goalNode = pathfinder.GetGrid[randomEndPos];
        
            List<NewNode> path = pathfinder.GetPath(startNode, goalNode);

            PrintPath(path);
        }
    
        public Dictionary<Vector2Int, NewNode> GetGrid => Grid;
    }
}
