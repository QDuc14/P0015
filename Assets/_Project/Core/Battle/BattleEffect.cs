namespace Project.Core.Battle
{
    public enum EffectTriggerType
    {
        Instance,
        TurnStart,
        TurnEnd,
        CircleStart,
        CircleEnd,
    }


    public abstract class BattleEffect
    {
        public abstract EffectTriggerType TriggerType { get; }
    }

}
