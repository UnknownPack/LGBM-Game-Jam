using UnityEngine;

public class StatusEffects
{
    protected int duration;
    protected TurnManager turnManager;
    protected GameObject target;
    protected bool isActive = true;
    
    public void Init(TurnManager turnManager, GameObject target, int duration)
    {
        this.turnManager = turnManager;
        this.target = target;
        this.duration = duration;
        AppyStatusEffect();
    }
    
    public int GetDuration => duration;
    public bool IsActive => isActive;
    public void tickDown()
    {
        Debug.Log(" Applying status effect");
        AppyStatusEffect();
        duration--;
        if (duration <= 0)
        {
            Debug.Log(" Deactivating status effect");
            DeactivateStatusEffect();
            isActive = false;
        }
    }
    public virtual void AppyStatusEffect()
    {
        return;
    }

    public virtual void DeactivateStatusEffect()
    {
    }
}

public class DefenceBoost : StatusEffects
{
    public override void AppyStatusEffect()
    {
        if (target == null)
        {
            Debug.LogError("Target is null");
            return;
        }
        
        BaseBattleEntity entity = target.GetComponent<BaseBattleEntity>();
        if(entity == null)
        {
            Debug.LogError("Target does not have a BaseBattleEntity component.");
            return;
        }
        
        entity.SetDefence(entity.GetIntialStats.Defence+3);
    }
    
    public override void DeactivateStatusEffect()
    {
        if (target == null)
        {
            Debug.LogError("Target is null");
            return;
        }
        
        BaseBattleEntity entity = target.GetComponent<BaseBattleEntity>();
        if(entity == null)
        {
            Debug.LogError("Target does not have a BaseBattleEntity component.");
            return;
        }
        
        entity.SetDefence(entity.GetIntialStats.Defence);
    }
}


public class Insult : StatusEffects
{
    public override void AppyStatusEffect()
    {
        if (target == null)
        {
            Debug.LogError("Target is null");
            return;
        }
        
        BaseBattleEntity entity = target.GetComponent<BaseBattleEntity>();
        if(entity == null)
        {
            Debug.LogError("Target does not have a BaseBattleEntity component.");
            return;
        }
        
        entity.SetDefence(1f);
    }
    
    public override void DeactivateStatusEffect()
    {
        if (target == null)
        {
            Debug.LogError("Target is null");
            return;
        }
        
        BaseBattleEntity entity = target.GetComponent<BaseBattleEntity>();
        if(entity == null)
        {
            Debug.LogError("Target does not have a BaseBattleEntity component.");
            return;
        }
        entity.SetDefence(entity.GetIntialStats.Defence);
    }
}

public class Barricade : StatusEffects
{
    private GameObject barricadePrefab;
    private GameObject barricadeInstance;
    private Node barricadeNode;
    
    public void SetBarricadePrefab(GameObject prefab) => barricadePrefab = prefab;
    
    public void CreateBarricade()
    {
        if(barricadeInstance == null)
        {
            Vector3 position = target.transform.position;
            barricadeInstance =
                UnityEngine.Object.Instantiate(barricadePrefab, position, Quaternion.identity);
            barricadeNode = turnManager.GetGridManager
                .GetNodeFromPosition(position);
            barricadeNode.SetWalkableState(false);
            barricadeNode.ManualUpdate = true;
            Debug.Log($"Node at: {barricadeNode.GetGridPosition} is set to {barricadeNode.CanNavigate}");
        }
    }

    public override void DeactivateStatusEffect()
    {
        if (barricadeInstance != null)
        {
            UnityEngine.Object.Destroy(barricadeInstance);
            barricadeNode.SetWalkableState(true);
            barricadeNode.ManualUpdate = false;
        }
    }
}
