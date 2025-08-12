using UnityEngine;

public class Node
{
    private Vector2Int gridPosition;
    private Vector3 realPosition;
    private GameObject _tileObject;
    private bool _canNavigateTo;

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

    public void RaiseBaricadeOnNode()
    {

    }

    public GameObject GetTileObject => _tileObject;
    public Vector3 GetRealPosition => realPosition;
    public Vector2Int GetGridPosition => gridPosition;
    public bool CanNavigate => _canNavigateTo;
}
