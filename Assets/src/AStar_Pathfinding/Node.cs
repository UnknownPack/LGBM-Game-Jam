using UnityEngine;

public class Node
{
    public Vector2Int gridPosition;
    public Vector2 realPosition;
    private GameObject _tileObject;
    public bool _canNavigateTo;

    public float GCost;
    public float HCost;
    public float FCost => GCost + HCost;
    
    public Node Parent;
    
    public Node(Vector2Int gridPosition, Vector3 realPosition, GameObject obj, bool canNavigateTo)
    {
        this.gridPosition = gridPosition;
        this.realPosition = realPosition;
        _tileObject = obj;
        _canNavigateTo = canNavigateTo;
        Parent = null;
    }
    
    public void SetWalkableState(bool state)
    {
        _canNavigateTo = state;
    }

    public GameObject GetTileObject => _tileObject;
    public Vector2 GetRealPosition => realPosition;
    public Vector2 GetGridPosition => gridPosition;
    public bool CanNavigate => _canNavigateTo;
}
