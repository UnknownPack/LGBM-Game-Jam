using UnityEngine;

public class Healer : BaseBattleEntity
{
    [Header("\nHealer Specfic Stats")]
    public float HealingRange = 3f;
    public float HealingAmount = 1f;
    public float BaricadeSummonRange = 3f;
    public float barricadeDuration = 2f;
    public GameObject BaricadePrefab;
    protected override void InitialiseActions()
    {
        base.InitialiseActions();
        HealAction healAction = new HealAction();
        healAction.SetHealthPoints(HealingAmount);
        InitActions(AbilityName.Heal, healAction, HealingRange, ActionPoint_Cost, ActionType.ActionPoint,
            ActionTargetType.Unit);
        
        RaiseBaricadeAction raiseBaricadeAction = new RaiseBaricadeAction();
        raiseBaricadeAction.SetBarricadeDuration(barricadeDuration);
        raiseBaricadeAction.SetBarricadePrefab(BaricadePrefab);
        InitActions(AbilityName.Raise_Baricade, raiseBaricadeAction, BaricadeSummonRange, ActionPoint_Cost, ActionType.ActionPoint,
            ActionTargetType.Tile);
        
        CastFireBall castFireBall = new CastFireBall();
        castFireBall.SetDuration(3);
        InitActions(AbilityName.FireCast, castFireBall, HealingRange, ActionPoint_Cost, ActionType.ActionPoint, ActionTargetType.Unit);
    }
}
