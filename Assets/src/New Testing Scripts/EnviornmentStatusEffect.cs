namespace src.New_Testing_Scripts
{
    public class EnviornmentStatusEffect 
    {
        private StatusEffectName statusEffectName;
        private int statusEffectNameDuration;
        
        public EnviornmentStatusEffect(StatusEffectName name, int duration)
        {
            statusEffectName = name;
            statusEffectNameDuration = duration;
        }
        
        public StatusEffectName StatusEffectName => statusEffectName;
        public int StatusEffectNameDuration
        {
            get => statusEffectNameDuration;
            set => statusEffectNameDuration = value;
        }
    }
}
