using UnityEngine;

public class Tile
{
    private Vector2 _position;
    private GameObject _tileObject;
    
    
    public Tile(GameObject tileObject, Vector2 position)
    {
        this._tileObject = tileObject;
        this._position = position;
    }

    public GameObject TileObject => _tileObject;
    public Vector2 Position => _position;
    
}
