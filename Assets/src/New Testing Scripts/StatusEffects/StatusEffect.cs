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
            Debug.Log($"{this} applied status affected to {entity.gameObject.name}! \n {scriptableObject.Duration} ticks left!");
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
                ResetStats();
                isActive = false;
                entity = null;
                ListenerManager.RemoveListener("Tick", OnTick); 
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
            float newValue = entity.GetHealth - scriptableObject.Value;
            entity.GetHealth = Mathf.Clamp(entity.GetHealth, 0, newValue);
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
            float newValue = entity.GetDamage - scriptableObject.Value;
            entity.GetDamage = Mathf.Clamp(entity.GetDamage, 0, newValue);
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
            int newValue = entity.GetMovementPoints - (int)scriptableObject.Value;
            entity.GetMovementPoints = Mathf.Clamp(entity.GetMovementPoints, 0, newValue);
        }

        public override void ResetStats()
        {
            entity.GetMovementPoints = (int)originalValue;
        }
    }
}
