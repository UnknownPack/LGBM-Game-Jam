using UnityEngine;

public class Node
{
    public Vector2 _position;
    public bool _canNavigateTo;

    public float GCost;
    public float HCost;
    public float FCost => GCost + HCost;
    
    public Node Parent;
    
    public Node(Vector2 position, bool canNavigateTo)
    {
        _position = position;
        _canNavigateTo = canNavigateTo;
        Parent = null;
        //Debug.Log($"Node created at: {_position}. Can navigate: {_canNavigateTo}");
    }
}
