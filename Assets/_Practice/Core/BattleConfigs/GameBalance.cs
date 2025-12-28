namespace Practice.Core
{
    public class GameBalance
    {
        public float CriticalHitChance { get; }
        public float CriticalHitMultiplier { get; }
        public float GlobalDamageModifier { get; }
        public float ExpModifier { get; }

        public GameBalance(float criticalHitChance, float criticalHitMultiplier, float globalDamageModifier, float expModifier)
        {
            CriticalHitChance = criticalHitChance;
            CriticalHitMultiplier = criticalHitMultiplier;
            GlobalDamageModifier = globalDamageModifier;
            ExpModifier = expModifier;
        }
    }
}
