using Practice.Core;
using System;

namespace Practice.Features.Battle
{
    public sealed class SkillLogic
    {
        private readonly GameBalance _gameBalance;
        private readonly Random _rng;

        public SkillLogic(GameBalance gameBalance)
        {
            _gameBalance = gameBalance ?? throw new ArgumentNullException(nameof(gameBalance));
            _rng = new Random();
        }

        public bool CanUseSkill(SKillDefinition sKill, BattleUnit user)
        {
            if (user == null || user.IsDead)
                return false;
            if (user.CurrentMp < sKill.MpCost)
                return false;
            return true;
        }

        public int CalDamge(SKillDefinition sKill, BattleUnit user, BattleUnit target)
        {
            if (user == null || target == null || user.IsDead || target.IsDead)
                return 0;
            int damage = user.Attack + sKill.Power;
            if (IsCriticalHit())
            {
                damage = (int)(damage * _gameBalance.CriticalHitMultiplier);
            }
            return damage;
        }

        private bool IsCriticalHit()
        {
            if (_gameBalance.CriticalHitChance <= 0)
                return false;
            if (_gameBalance.CriticalHitChance >= 1)
                return true;

            double roll = _rng.NextDouble();
            return roll < _gameBalance.CriticalHitChance;

        }

        public int CalHeal(SKillDefinition sKill, BattleUnit user)
        {
            if (user == null || user.IsDead)
                return 0;
            int healAmount = sKill.Power;
            return healAmount;
        }

        public int CalMpCost(SKillDefinition sKill, BattleUnit user)
        {
            if (user == null || user.IsDead)
                return 0;
            int cost = sKill.MpCost;
            return cost;
        }
    }
}
