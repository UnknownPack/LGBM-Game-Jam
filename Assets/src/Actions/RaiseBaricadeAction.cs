using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiseBaricadeAction : ActionBase
{
    public void SetBarricadeDuration(float duration) => this.duration = duration;
    private float duration = 3f;
    
    private GameObject barricadePrefab;
    public void SetBarricadePrefab(GameObject prefab) => barricadePrefab = prefab;
    
    public override IEnumerator Action(GameObject target)
    {
        Barricade barricade = new Barricade();
        barricade.Init(turnManager, target, (int)duration);
        barricade.SetBarricadePrefab(barricadePrefab);
        barricade.CreateBarricade();
        
        Debug.Log("Baricades raised in the area!");
        //TODO: TRIGGER ANIMATION(S) HERE
        
        // Remove action points etc.
        yield return base.Action(target);
    }
    
    public override ActionTargetType GetActionTargetType => ActionTargetType.Tile;
}
