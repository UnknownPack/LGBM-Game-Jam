using System.Collections;
using UnityEngine;

public class ActionBase
{
    protected GameObject ParentObject;
    protected int ActionCost = 1; 

    public virtual void Init(GameObject parentObject) => ParentObject = parentObject;

    public virtual IEnumerator Action()
    {
        yield return null;
    }
}
