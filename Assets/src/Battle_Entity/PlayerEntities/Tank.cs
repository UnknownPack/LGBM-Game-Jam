using UnityEngine;

public class Tank : BaseBattleEntity
{
    
    private int timesHit = 0;
    protected override void InitialiseActions()
    {
        base.InitialiseActions();
        
    }

    public override void TakeDamage(float damageAmount)
    {
        // Calculate effective damage considering defence
        timesHit ++;
        Damage += timesHit * 0.5f; // Increase damage by 0.5 for each hit
        base.TakeDamage(damageAmount);
    }
}
