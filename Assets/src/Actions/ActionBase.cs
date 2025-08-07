using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBase
{
    protected GameObject ParentObject;
    protected float ActionRange;
    protected ActionType _actionType;
    protected ActionTargetType _actionTargetType;
    protected int ActionCost; 
    protected PathFinding PathFinder;
    protected Dictionary<Vector2Int, Node> Grid_Nodes;
    protected GridManager _gridManager;
    protected BaseBattleEntity _baseBattleEntity;

    public virtual void Init(GameObject parentObject, BaseBattleEntity _baseBattleEntity, float actionRange, int ActionCost, ActionTargetType _actionTargetType ,ActionType _actionType, GridManager gridManager)
    { 
        ParentObject = parentObject;
        this._baseBattleEntity = _baseBattleEntity;
        ActionRange  = actionRange;
        this.ActionCost = ActionCost;
        this._actionType = _actionType;
        this._actionTargetType = _actionTargetType;
        this._gridManager = gridManager;
        this.PathFinder = _gridManager.GetPathFinding;
        this.Grid_Nodes = _gridManager.GetGridNodes;
    }

    public virtual IEnumerator Action(GameObject target)
    {
        yield return null;
        _baseBattleEntity.RemoveActionPoint(_actionType, ActionCost);
        Debug.Log($"{_actionType.ToString()}s remaining: {_baseBattleEntity.GetActionPointsCount(_actionType).ToString()} ");
    }

    public void ShowActionRange(Color highlightColor)
    {
        foreach (var node in Grid_Nodes)
        {
            if (TargetWiihinRange(node.Value.GetTileObject))
            {
                SpriteRenderer spriteRenderer = node.Value.GetTileObject.GetComponent<SpriteRenderer>();
                if (spriteRenderer == null)
                    Debug.LogError("SpriteRender not found");
                
                spriteRenderer.color = highlightColor;
            }
        }
        _gridManager.GetNodeFromPosition(ParentObject.transform.position).GetTileObject.GetComponent<SpriteRenderer>().color = Color.gray;
    }

    public bool TargetWiihinRange(GameObject target)
    {
        Vector3 ParentPosition = ParentObject.transform.position;
        Vector3 TargetPostion = target.transform.position;
        Node CurrentNode = _gridManager.GetNodeFromPosition(ParentPosition);
        Node TargetNode = _gridManager.GetNodeFromPosition(TargetPostion); 
        return Vector2Int.Distance(TargetNode.GetGridPosition, CurrentNode.GetGridPosition) <= ActionRange;
    }

    protected List<Node> GetListOfNodesAffected()
    {
        List<Node> output = new List<Node>();
        foreach (var node in Grid_Nodes)
        {
            if (TargetWiihinRange(node.Value.GetTileObject))
                output.Add(node.Value);
        }
        return output;
    }

    public ActionType GetActionType => _actionType;

}

[System.Serializable]
public enum ActionType
{
    MovePoint,
    ActionPoint
}

[System.Serializable]
public enum ActionTargetType
{
    Unit,
    Tile
}
