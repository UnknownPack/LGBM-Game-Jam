using UnityEngine;

public class StatusEffects
{
    protected int duration;
    protected TurnManager manager;
    
    public void Init(TurnManager turnManager, int duration)
    {
        this.manager = turnManager;
        this.duration = duration;
    }
    
    public void Update() => duration--;
    public int GetDuration => duration;
    
    public virtual void AppyStatusEffect(BaseBattleEntity entity)
    {
        return;
    }
}

[System.Serializable]
public enum StatusEffectType
{
    Insult
}
