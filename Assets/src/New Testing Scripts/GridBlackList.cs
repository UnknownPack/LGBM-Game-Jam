using UnityEngine;

namespace src.New_Testing_Scripts.Editor
{
    public class GridBlackList : MonoBehaviour
    {
        public int gridWidth = 5;
        public int gridHeight = 5;

        public bool[,] blacklist;
    
        void OnValidate()
        {
            if (blacklist == null || blacklist.GetLength(0) != gridWidth || blacklist.GetLength(1) != gridHeight)
            {
                blacklist = new bool[gridWidth, gridHeight];
            }
        }
    }
}
