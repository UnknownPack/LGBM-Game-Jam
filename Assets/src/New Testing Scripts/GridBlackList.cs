using System.Collections.Generic;
using UnityEngine;

namespace src.New_Testing_Scripts
{
    [CreateAssetMenu(fileName = "GridBlackListAsset", menuName = "Grids/Grid Blacklist", order = 1)]
    public class GridBlackListSo : ScriptableObject
    {
        [Header("Grid Settings")]
        public int gridWidth = 5;
        public int gridHeight = 5;

        [Header("Layers")]
        public int layerCount = 4;

        [HideInInspector] 
        public List<BoolGrid> blacklists = new List<BoolGrid>();

        void OnValidate()
        {
            if (blacklists == null)
                blacklists = new List<BoolGrid>();

            // Ensure correct number of layers
            while (blacklists.Count < layerCount)
                blacklists.Add(new BoolGrid(gridWidth, gridHeight));
            while (blacklists.Count > layerCount)
                blacklists.RemoveAt(blacklists.Count - 1);

            // Ensure each layer matches dimensions
            foreach (var layer in blacklists)
            {
                if (layer.cells == null || 
                    layer.width != gridWidth || 
                    layer.height != gridHeight)
                {
                    layer.cells = new bool[gridWidth, gridHeight]; // <-- reinitialize
                }

                layer.width = gridWidth;
                layer.height = gridHeight;
            }
        }
    }
    
    [System.Serializable]
    public class BoolGrid
    {
        public int width;
        public int height;
        public LayerType layerType;
        public bool[,] cells;

        public BoolGrid(int w, int h)
        {
            width = w; height = h;
            cells = new bool[w, h];
        }
    }

    [System.Serializable]
    public enum LayerType
    {
        Spawn_Player, Spawn_Enemy, Obstacle, NonNavigable
    }
}
