using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace src.New_Testing_Scripts.TileMapTesting
{
    public class NewNode
    {
        private Vector2Int gridPosition;
        private Vector3 realPosition;
        private TileBase tile;
        private bool _canNavigateTo;
        private List<EnviornmentStatusEffect> statusEffects= new List<EnviornmentStatusEffect>();

        public float GCost;
        public float HCost;
        public float FCost => GCost + HCost;
    
        public NewNode Parent;
    
        public NewNode(Vector2Int gridPosition, Vector3 realPosition, TileBase tile, bool canNavigateTo)
        {
            this.gridPosition = gridPosition;
            this.realPosition = realPosition;
            this.tile = tile;
            _canNavigateTo = canNavigateTo;
            Parent = null;
            
            var tickManager = ServiceLocator.Get<ListenerManager>();
            
            if (tickManager == null)
            {
                Debug.LogError("There is no listener manager for status effect.");
                return;
            }
            tickManager.AddListener("Tick", OnTick);
            
        }
        
        private void OnTick()
        {
            for (int i = statusEffects.Count - 1; i >= 0; i--)
            {
                EnviornmentStatusEffect effect = statusEffects[i];
                effect.StatusEffectNameDuration--;

                if (effect.StatusEffectNameDuration <= 0)
                {
                    Debug.Log($"{effect.StatusEffectName} has expired at {gridPosition}");
                    statusEffects.RemoveAt(i);
                }
            }
        }
    
        public void SetWalkableState(bool state)
        {
            _canNavigateTo = state;
        }  
    
        public void AddStatusEffect(EnviornmentStatusEffect statusEffect) => statusEffects.Add(statusEffect);
        
        public void GiveTargetStatusEffects(NewEntityBase target)
        {
            foreach (EnviornmentStatusEffect statusEffect in statusEffects)
            {
                StatusEffectName name = statusEffect.StatusEffectName;
                if(target.ContainsEffeccct(name) && StatusEffectManager.FindByName(name).DoesStack)
                    continue;
                    
                StatusEffectManager.Create(statusEffect.StatusEffectName, target);
            } 
        }

        public List<EnviornmentStatusEffect> GetStatusEffects => statusEffects;
        public TileBase GetTile => tile;
        public Vector3 GetRealPosition => realPosition;
        public Vector2Int GetGridPosition => gridPosition;
        public bool CanNavigate => _canNavigateTo;
    }
}
