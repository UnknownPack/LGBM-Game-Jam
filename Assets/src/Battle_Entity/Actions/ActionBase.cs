using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBase
{
    protected GameObject ParentObject;
    protected float ActionRange;
    protected int ActionCost = 1; 
    protected PathFinding PathFinder;
    protected Dictionary<Vector2Int, Node> Grid_Nodes;
    protected GridManager _gridManager;

    public virtual void Init(GameObject parentObject, float actionRange, GridManager gridManager)
    { 
        ParentObject = parentObject;
        ActionRange  = actionRange;
        this._gridManager = gridManager;
        this.PathFinder = _gridManager.GetPathFinding;
        this.Grid_Nodes = _gridManager.GetGridNodes;
    }

    public virtual void SetVariables(){}

    public virtual IEnumerator Action(GameObject target){yield return null;}

    public void ShowActionRange()
    {
        foreach (var node in Grid_Nodes)
        {
            if (TargetWiihinRange(node.Value.GetTileObject))
            {
                SpriteRenderer spriteRenderer = node.Value.GetTileObject.GetComponent<SpriteRenderer>();
                if (spriteRenderer == null)
                    Debug.LogError("SpriteRender not found");
                
                spriteRenderer.color = Color.cyan;
            }
        }
    }

    public bool TargetWiihinRange(GameObject target)
    {
        Vector3 ParentPosition = ParentObject.transform.position;
        Vector3 TargetPostion = target.transform.position;
        Node CurrentNode = _gridManager.GetNodeFromPosition(ParentPosition);
        Node TargetNode = _gridManager.GetNodeFromPosition(TargetPostion); 
        return Vector2Int.Distance(TargetNode.GetGridPosition, CurrentNode.GetGridPosition) <= ActionRange;
    }

}

public enum ActionType
{
    MovePoint,
    ActionPoint
}
