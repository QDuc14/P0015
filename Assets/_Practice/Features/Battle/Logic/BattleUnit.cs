using Practice.Core.Stats;

namespace Practice.Features.Battle
{
    public sealed class BattleUnit
    {
        public UnitDefinition Definition { get; }

        public string Name => Definition.DisplayName;
        public int MaxHp => Definition.MaxHp;
        public int Attack => Definition.Attack;

        public int CurrentHp { get; private set; }

        public bool IsDead => CurrentHp <= 0;

        public BattleUnit(UnitDefinition definition)
        {
            Definition = definition;
            CurrentHp = MaxHp;
        }

        public void TakeDamage(int damage)
        {
            if (damage <= 0 || IsDead)
                return;
            CurrentHp -= damage;
            if (CurrentHp < 0)
                CurrentHp = 0;
        }

        public void Heal(int amount)
        {
            if (amount <= 0 || IsDead)
                return;
            CurrentHp += amount;
            if (CurrentHp > MaxHp)
                CurrentHp = MaxHp;
        }
    }
}
