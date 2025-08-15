using NUnit.Framework;
using UnityEngine;

public class DamageGuy : BaseBattleEntity
{
    [Header("\nDamage Stats")]
    public float AttackRange = 1f;
    public float grenadeRange = 2f;
    public int insultDuration = 2;
    public GameObject grenadePrefab;
    public float extraDamageBuildUp = 0f; 
    
    public override float GetDamage()
    {
        float output = Damage + extraDamageBuildUp + (greedKillCounter * 0.5f);
        extraDamageBuildUp = 0;
        return output;
    }
    
    public override void TakeDamage(float damageAmount)
    {
        bool HasParried = Random.value < 0.5f;
        if (HasParried)
        {
            GetAnimator.Play("Attack");
            extraDamageBuildUp += damageAmount * 0.5f; // Build up extra damage on parry
            return;
        }
        greedKillCounter = 0; // Reset greed counter on taking damage
        Defence = Mathf.Clamp(Defence--, 0, Defence); // Decrease defence when taking damage
        base.TakeDamage(damageAmount);
    }
    
    public void IncreaseGreedCounter(int amount) => greedKillCounter += amount; 
    public int greedKillCounter = 0;

    private void Awake()
    {
        SetGrenadePrefab(grenadePrefab);
    }
    protected override void InitialiseActions()
    {
        base.InitialiseActions();
        
        MeleeAttackAction meleeAttackAction = new MeleeAttackAction();
        meleeAttackAction.SetDamageAmount(GetDamage());
        meleeAttackAction.SetTargetType(UnitOwnership.Enemy);
        InitActions(AbilityName.Attack, meleeAttackAction, AttackRange, ActionPoint_Cost, ActionType.ActionPoint,
            ActionTargetType.Unit);
        
        InitActions(AbilityName.Grenade, new GrenadeAction(), grenadeRange, ActionPoint_Cost, ActionType.ActionPoint, ActionTargetType.Tile);
        
        InsultAction insultAction = new InsultAction();
        insultAction.SetInsultDuration(insultDuration);
        InitActions(AbilityName.Insult, insultAction, grenadeRange, ActionPoint_Cost, ActionType.ActionPoint, ActionTargetType.Unit);
    }

}
