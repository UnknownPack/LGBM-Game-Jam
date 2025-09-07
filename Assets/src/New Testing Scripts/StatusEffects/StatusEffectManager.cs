using System;
using System.Collections.Generic;
using UnityEngine;

namespace src.New_Testing_Scripts
{
    public class StatusEffectManager: MonoBehaviour
    {
        public List<StatusEffect_SO> StatusEffectsDefinitions = new List<StatusEffect_SO>();

        private Dictionary<StatusEffectName, Func<NewEntityBase, StatusEffect_SO, NewStatusEffect>>
            StatusEffectCreators
                = new Dictionary<StatusEffectName, Func<NewEntityBase, StatusEffect_SO, NewStatusEffect>>
                {
                    { StatusEffectName.DamageTarget, (entity, so) => new DamageTarget(entity, so) },
                    { StatusEffectName.Slow, (entity, so) => new Slow(entity, so) },
                    { StatusEffectName.DamageBoost, (entity, so) => new DamageBoost(entity, so) },
                };

        private void Awake()
        {
            ServiceLocator.Register(this);
        }

        public  void Create(StatusEffectName so, NewEntityBase entity)
        {
            if (StatusEffectCreators.ContainsKey(so))
            {
                StatusEffect_SO original = FindByName(so);
                StatusEffect_SO instance = ScriptableObject.Instantiate(original);

                entity.AddStatusEffect(StatusEffectCreators[so](entity, instance));
                return;
            }
                
            Debug.LogError($"No status effect registered for {so}");
        }
        
        public StatusEffect_SO FindByName(StatusEffectName name)
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
