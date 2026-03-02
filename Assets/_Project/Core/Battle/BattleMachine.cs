using System;

namespace Project.Core.Battle
{
    public sealed class BattleMachine
    {
        public BattleState State { get; } = new BattleState();

        public event Action CharacterSelected;
        public event Action<NormalAction> NormalActionSelected;
        public event Action<SkillAction> SkillActionSelected;

        public BattleMachine()
        {
            State.CircleIndex = 1;
        }

        public void Start() 
        {
            State.CirclePhase = BattlePhase.StartCircle;
            StateProceed();
        }

        public void SelectCharacter()
        {
            State.CirclePhase = BattlePhase.SelectCharacter;
            CharacterSelected?.Invoke();
        }

        public void RequestAction(BattleAction action)
        {
            State.CirclePhase = BattlePhase.SelectAction;
            switch (action)
            {
                case NormalAction nor:
                    NormalActionSelected?.Invoke(nor);
                    break;

                case SkillAction skl:
                    SkillActionSelected?.Invoke(skl);
                    break;
            }
        }

        private void StateProceed()
        {
            switch (State.CirclePhase)
            {
                case BattlePhase.StartCircle:

                    break;

                case BattlePhase.EndCircle:

                    break;
            } 
        }
    }
}
