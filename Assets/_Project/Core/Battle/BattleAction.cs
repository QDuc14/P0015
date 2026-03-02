using System;
using System.Collections.Generic;

namespace Project.Core.Battle
{
    public enum ActionType
    {
        Normal,
        Skill,
        Special,
    }

    public readonly struct Normal
    {
        public readonly string Id;
        public readonly string Name;

        public Normal(string id, string name)
        {
            Id = id ?? ""; 
            Name = name ?? "";
        }
    }

    public readonly struct Skill
    {
        public readonly string Id;
        public readonly string Name;
        public readonly IReadOnlyList<BattleEffect> Effects;

        public Skill(string id, string name, IReadOnlyList<BattleEffect> effects)
        {
            Id = id ?? "";
            Name = name ?? "";
            Effects = effects ?? Array.Empty<BattleEffect>();
        }
    }

    public abstract class BattleAction
    {
        public abstract ActionType ActionType { get; }
    }

    public class NormalAction : BattleAction 
    { 
        public override ActionType ActionType { get => ActionType.Normal; }
        public Normal Action { get; }
        public NormalAction(Normal action) 
        { 
            Action = action;
        }
    }

    public class SkillAction : BattleAction
    {
        public override ActionType ActionType { get => ActionType.Skill; }
        public Skill Action { get; }
        public SkillAction(Skill action)
        {
            Action = action;
        }
    }
}