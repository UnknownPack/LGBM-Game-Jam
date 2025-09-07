using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace src.New_Testing_Scripts.TileMapTesting
{
    public class GridSystem : MonoBehaviour
    {
        public GameObject tilePrefab;
        public Tilemap Tilemap;
        private Dictionary<NewEntityBase,Vector2Int> ActiveEntites = new Dictionary< NewEntityBase, Vector2Int>();
        private NewPathfinder pathfinder;
        Dictionary<Vector2Int, NewNode> Grid;

        private void Awake()
        {
            ServiceLocator.Register(this);
            Debug.Log("Added Grid Systen");
            Dictionary<Vector2Int, NewNode> grid = GenerateMap();
            Grid = grid;
            pathfinder = new NewPathfinder(grid);
        }

        void Start()
        {
            GameObject previousParent = GameObject.Find("ParentObject");
            if (previousParent != null)
                DestroyImmediate(previousParent); 
        }

        private void Update()
        {
            
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
                    // ✅ FIX: use the cell center, not bottom-left corner
                    Vector3 realPosition = Tilemap.GetCellCenterWorld(pos);
                    NewNode newNode = new NewNode(gridPosition, realPosition, tile, true);
                    Grid[gridPosition] = newNode;
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

        public NewNode GetNodeAtWorld(Vector3 worldPos)
        {
            Vector3Int cell = Tilemap.WorldToCell(worldPos);                 // ✅ use the tilemap for conversion
            var key = new Vector2Int(cell.x, cell.y);
            return GetGrid.TryGetValue(key, out var node) ? node : null;
        }

        public void UpdateEntityPosition(NewEntityBase entityBase, Vector2Int newPos) 
        {
            ActiveEntites[entityBase] = newPos;
            Grid[newPos].GiveTargetStatusEffects(entityBase);
        }
        
        public Dictionary<Vector2Int, NewNode> GetGrid => Grid;
        public List<NewNode> GetPath(NewNode startNode, NewNode goalNode) => pathfinder.GetPath(startNode, goalNode);
    }
}
