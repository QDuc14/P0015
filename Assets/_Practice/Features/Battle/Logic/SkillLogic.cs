using Practice.Core;

namespace Practice.Features.Battle
{
    public sealed class SkillLogic
    {
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
            return damage;
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
