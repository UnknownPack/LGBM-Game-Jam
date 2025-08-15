using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffects
{
    protected int duration;
    protected TurnManager turnManager;
    protected GameObject target;
    protected bool isActive = true;
    protected Coroutine coroutine;
    
    public void Init(TurnManager turnManager, GameObject target, int duration)
    {
        this.turnManager = turnManager;
        this.target = target;
        this.duration = duration;
        turnManager.AddStatusEffect(this);
        AppyStatusEffect();
    }
    public bool IsActive => isActive;
    public void tickDown()
    {
        if(!isActive)
            return;
        
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
        return;
    }
    
    protected IEnumerator HighlightStatusEffect(Color colorOne, Color colorTwo)
    {
        SpriteRenderer spriteRenderer = target.GetComponent<SpriteRenderer>();
        float duration = 0.5f, elaspedTime;
        while (true)
        {
            elaspedTime = 0f;
            while (elaspedTime < duration)
            {
                spriteRenderer.color = Color.Lerp(colorOne, colorTwo, elaspedTime/duration);
                elaspedTime += Time.deltaTime;
                yield return null;
            }
            
            spriteRenderer.color = colorTwo;
            elaspedTime = 0f;
            yield return new WaitForSeconds(0.25f);
            
            while (elaspedTime < duration)
            {
                spriteRenderer.color = Color.Lerp(colorTwo, colorOne, elaspedTime/duration);
                elaspedTime += Time.deltaTime;
                yield return null;
            }
            
            spriteRenderer.color = colorOne;
            yield return new WaitForSeconds(0.25f);
        }
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

public class FireMelt : StatusEffects
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
        
        entity.GetAnimator.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        entity.TakeDamage(0.5f);
    }
    
    public override void DeactivateStatusEffect()
    {
        if (target == null)
        {
            Debug.LogWarning("Target is null");
            return;
        }

        if (target.GetComponent<BaseBattleEntity>() == null)
        {
            Debug.LogWarning("Target does not have a BaseBattleEntity component.");
            return;
        }

        if (target.GetComponent<BaseBattleEntity>().GetAnimator.gameObject.GetComponent<SpriteRenderer>() == null)
        {
            Debug.LogWarning("Target does not have a SpriteRenderer component.");
            return;
        }
        
        target.GetComponent<BaseBattleEntity>().GetAnimator.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
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
