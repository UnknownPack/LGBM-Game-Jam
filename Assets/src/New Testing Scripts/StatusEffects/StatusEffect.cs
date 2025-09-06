using UnityEngine;

namespace src.New_Testing_Scripts
{
    public enum StatusEffectName
    {
        DamageTarget, Slow, DamageBoost 
    }
    
    
    public abstract class NewStatusEffect
    {
        protected StatusEffect_SO scriptableObject;
        protected bool isActive = true;
        protected NewEntityBase entity;
        protected float originalValue = 0f;

        public NewStatusEffect(NewEntityBase entity, StatusEffect_SO scriptableObject)
        {
            this.entity = entity;
            this.scriptableObject = scriptableObject;
            ListenerManager.AddListener("Tick", OnTick);
            Apply(this.entity.gameObject);
        }
        
        private void OnTick()
        {
            if(!isActive)
            {
                Debug.LogError($"This status effect {scriptableObject.statusEffectName} is not active!");
                return;
            }
            if(scriptableObject.DoesTick && scriptableObject.Duration % scriptableObject.TickInterval == 0)
                Apply(entity.gameObject);
            Tick();                   
        }
        
        public virtual void Apply(GameObject target)
        {
            if(entity == null)
            {
                Debug.LogError("Entity not set for status effect!");
                return;
            }

            
            //Apply effect logic here
        }
        
        public void Tick() 
        {
            scriptableObject.Duration -= 1;
            if (scriptableObject.Duration <= 0)
            {
                isActive = false;
                entity = null;
                ListenerManager.RemoveListener("Tick", OnTick);
                ResetStats();
            }
        }

        public abstract void ResetStats();
        
        public StatusEffectName GetStatusEffectName => scriptableObject.statusEffectName;
        public bool IsActive => isActive;
        public float GetDuration => scriptableObject.Duration;
    }
    
    public class DamageTarget : NewStatusEffect
    {
     
        public DamageTarget(NewEntityBase entity, StatusEffect_SO scriptableObject) : base(entity, scriptableObject)
        {
        }

        
        public override void Apply(GameObject target)
        {
            entity.GetHealth -= scriptableObject.Value;
        }

        public override void ResetStats()
        {
            return;
        }
    }
    
    public class DamageBoost : NewStatusEffect
    {
     
        public DamageBoost(NewEntityBase entity, StatusEffect_SO scriptableObject) : base(entity, scriptableObject)
        {
            originalValue = entity.GetInitialStats.Damage;
        }

        
        public override void Apply(GameObject target)
        {
            entity.GetDamage = scriptableObject.Value;
        }

        public override void ResetStats()
        {
            entity.GetDamage = originalValue;
        }
    }
    
    public class Slow : NewStatusEffect
    {
     
        public Slow(NewEntityBase entity, StatusEffect_SO scriptableObject) : base(entity, scriptableObject)
        {
            originalValue = entity.GetInitialStats.Damage;
        }

        
        public override void Apply(GameObject target)
        {
            entity.GetMovementPoints -= (int)scriptableObject.Value;
        }

        public override void ResetStats()
        {
            entity.GetMovementPoints = (int)originalValue;
        }
    }
}
