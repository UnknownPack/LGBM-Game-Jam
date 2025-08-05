using System;
using System.Collections;
using UnityEngine;

public class ActionBase
{
    protected GameObject ParentObject;
    protected int ActionCost = 1; 

    public virtual void Init(GameObject parentObject) => ParentObject = parentObject;

    public virtual void SetVariables()
    {}

    public virtual IEnumerator Action(GameObject targert)
    {
        yield return null;
    }
}

public enum ActionType
{
    MovePoint,
    ActionPoint
}
