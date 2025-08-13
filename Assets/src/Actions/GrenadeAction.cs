using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAction : ActionBase
{
    private int blastRadius = 2;
    private int blastDamageMultiplier = 3;
    private GameObject grenadeInstance;
    private GameObject grenadePrefab;

    public override IEnumerator Action(GameObject target)
    {

        List<Node> NodesWithinAoe = GetNodesWithinAoe(target, blastRadius);
        Dictionary<Vector2Int, BaseBattleEntity> EntityGridPostions = _gridManager.GetEntityListToGrid();
        foreach (var node in NodesWithinAoe)
        {
            Vector2Int nodePosition = node.GetGridPosition;
            if(EntityGridPostions.ContainsKey(nodePosition))
                EntityGridPostions[nodePosition].TakeDamage(_baseBattleEntity.GetDamage * blastDamageMultiplier);
        }

        PlayAbilityAnimation("Throw");
        yield return new WaitForSeconds(1f);
        grenadePrefab = _baseBattleEntity.GetGrenadePrefab();
        Vector3 position = target.transform.position;
        grenadeInstance =
            UnityEngine.Object.Instantiate(grenadePrefab, position, Quaternion.identity);
        Object.Destroy(grenadeInstance, 1f);
        // Remove action points etc.
        yield return base.Action(target);
    }
    
}
