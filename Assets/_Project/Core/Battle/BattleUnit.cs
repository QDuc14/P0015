using System;
using System.Collections.Generic;

namespace Project.Core.Battle
{
    public enum OperatorType
    {
        Player,
        Enemy
    }

    public readonly struct PlayerOperator
    {
        public readonly string Id;
        public readonly string Name;
        public readonly int Hp;
        public readonly int Atk;
        public readonly int Def;
        public readonly int Mp;
        public readonly int Cp; // percentage

        public readonly IReadOnlyList<BattleEffect> StatusEffects;

        public PlayerOperator (string id, string name, int hp, int atk, int def, int mp, int cp, IReadOnlyList<BattleEffect> statusEffects)
        {
            Id = id ?? "";
            Name = name ?? "";
            Hp = hp < 0 ? 0 : hp;
            Atk = atk < 0 ? 0 : atk;
            Def = def < 0 ? 0 : def;
            Mp = mp < 0 ? 0 : mp;
            Cp = Math.Clamp(cp, 0, 100);
            StatusEffects = statusEffects ?? Array.Empty<BattleEffect>();
        }
    }

    public readonly struct EnemyOperator
    {
        public readonly string Id;
        public readonly string Name;
        public readonly int Hp;
        public readonly int Atk;
        public readonly int Def;
        public readonly int Bp;
        public readonly int Cp; // 0-8

        public readonly IReadOnlyList<BattleEffect> StatusEffects;

        public EnemyOperator (string id, string name, int hp, int atk, int def, int bp, int cp, IReadOnlyList<BattleEffect> statusEffect)
        {
            Id = id ?? "";
            Name = name ?? "";
            Hp = hp < 0 ? 0 : hp;
            Atk = atk < 0 ? 0 : atk;
            Def = def < 0 ? 0 : def;
            Bp = bp < 0 ? 0 : bp;
            Cp = Math.Clamp(cp, 0, 8);
            StatusEffects = statusEffect ?? Array.Empty<BattleEffect>();
        }
    }
    public abstract class BattleUnit
    {
        public abstract OperatorType Type { get; }
    }

    public class PlayerUnit : BattleUnit
    {
        public override OperatorType Type { get => OperatorType.Player; }
        public PlayerOperator Character { get; }
        public PlayerUnit(PlayerOperator character)
        {
            Character = character;
        }
    }

    public class EnemyUnit : BattleUnit
    {
        public override OperatorType Type { get => OperatorType.Enemy; }
        public EnemyOperator Character { get; }
        public EnemyUnit(EnemyOperator character)
        {
            Character = character;
        }
    }
}
