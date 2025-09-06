using System;
using System.Collections.Generic;
using UnityEngine;

namespace src.New_Testing_Scripts
{
    public static class StatusEffectManager
    {
        public static List<StatusEffect_SO> StatusEffectsDefinitions = new List<StatusEffect_SO>();

        private static Dictionary<StatusEffectName, Func<NewEntityBase, StatusEffect_SO, NewStatusEffect>>
            StatusEffectCreators
                = new Dictionary<StatusEffectName, Func<NewEntityBase, StatusEffect_SO, NewStatusEffect>>
                {
                    { StatusEffectName.DamageTarget, (entity, so) => new DamageTarget(entity, so) },
                    { StatusEffectName.Slow, (entity, so) => new Slow(entity, so) },
                    { StatusEffectName.DamageBoost, (entity, so) => new DamageBoost(entity, so) },
                };

        public static void Create(StatusEffectName so, NewEntityBase entity)
        {
            if (StatusEffectCreators.ContainsKey(so))
            {
                entity.AddStatusEffect(StatusEffectCreators[so](entity, FindByName(so)));
                return;
            }
                
            Debug.LogError($"No status effect registered for {so}");
        }
        
        public static StatusEffect_SO FindByName(StatusEffectName name)
        {
            foreach (var statusEffect in StatusEffectsDefinitions)
            {
                if (statusEffect.statusEffectName == name)
                    return statusEffect;
            }
            Debug.LogError($"Status effect {name} not found in definitions.");
            return null;
        }
    }
}
