using UnityEngine;
using UnityEngine.Tilemaps;

public class NewNode
{
    private Vector2Int gridPosition;
    private Vector3 realPosition;
    private TileBase tile;
    private bool _canNavigateTo;

    public float GCost;
    public float HCost;
    public float FCost => GCost + HCost;
    
    public NewNode Parent;
    
    public NewNode(Vector2Int gridPosition, Vector3 realPosition, TileBase tile, bool canNavigateTo)
    {
        this.gridPosition = gridPosition;
        this.realPosition = realPosition;
        this.tile = tile;
        _canNavigateTo = canNavigateTo;
        Parent = null;
    }
    
    public void SetWalkableState(bool state)
    {
        _canNavigateTo = state;
    }  

    public TileBase GetTile => tile;
    public Vector3 GetRealPosition => realPosition;
    public Vector2Int GetGridPosition => gridPosition;
    public bool CanNavigate => _canNavigateTo;
}
