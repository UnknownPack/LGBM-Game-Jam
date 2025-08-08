using UnityEngine;

public class Healer : BaseBattleEntity
{
    [Header("\nHealer Specfic Stats")]
    public float HealingRange = 3f;
    public float HealingAmount = 1f;
    public float BaricadeSummonRange = 3f;
    public float baricadeRadius = 2f;
    protected override void InitialiseActions()
    {
        base.InitialiseActions();
        HealAction healAction = new HealAction();
        healAction.SetHealthPoints(HealingAmount);
        InitActions(AbilityName.Heal, healAction, HealingRange, ActionPoint_Cost, ActionType.ActionPoint,
            ActionTargetType.Unit);
        
        RaiseBaricadeAction raiseBaricadeAction = new RaiseBaricadeAction();
        raiseBaricadeAction.SetBaricadeRadius(baricadeRadius);
        InitActions(AbilityName.RaiseBaricade, healAction, BaricadeSummonRange, ActionPoint_Cost, ActionType.ActionPoint,
            ActionTargetType.Tile);
    }
}
