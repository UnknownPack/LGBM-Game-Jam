using UnityEngine;

namespace src.New_Testing_Scripts
{
    [CreateAssetMenu(fileName = "StatusEffect_SO", menuName = "Scriptable Objects/StatusEffect_SO")]
    public class StatusEffect_SO : ScriptableObject
    {
        public StatusEffectName statusEffectName;
        public bool DoesTick, DoesStack;
        public int Duration, TickInterval;
        public float Value;
    }
}
