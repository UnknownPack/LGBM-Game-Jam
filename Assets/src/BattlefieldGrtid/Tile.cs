using UnityEngine;

public class Tile
{
    private Vector2 _position;
    private GameObject _tileObject;
    private bool _isWalkable = true;
    
    
    public Tile(GameObject tileObject, Vector2 position)
    {
        this._tileObject = tileObject;
        this._position = position;
    }

    public void SetWalkableState(bool state)
    {
        _isWalkable = state;
    }

    public GameObject TileObject => _tileObject;
    public Vector2 Position => _position;
    public bool IsWalkable => _isWalkable;
}
