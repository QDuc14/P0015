using System;

namespace Project.Core.Battle
{
    public sealed class BattleMachine
    {
        public BattleState State { get; } = new BattleState();

        public event Action PlayerTurnStarted;
        public event Action CharacterSelected;
        public event Action<NormalAction> NormalActionSelected;
        public event Action<SkillAction> SkillActionSelected;
        public event Action<SpecialAction> SpecialActionSelected;
        public event Action PlayerTurnEnded;
        public event Action EnemyTurnStarted;

        private bool _started;

        public BattleMachine()
        {
            State.CircleIndex = 0;
        }

        public void Start() 
        {
            if (_started) throw new InvalidOperationException("BattleMachine.Start() can only be called once.");

            _started = true;
            State.CirclePhase = BattlePhase.StartCircle;
            StateProceed();
        }

        public void SelectCharacter(BattleUnit unit)
        {
            if (!EnsurePhase(BattlePhase.PlayerTurn)
                || unit == null 
                || unit is not PlayerUnit pu 
                || pu.Character.Budget == 0)
            {
                throw new InvalidOperationException("BattleMachine.SelectCharacter() requires PlayerTurn, Player Unit, and Unit Budget > 0.");
            }
            CharacterSelected?.Invoke();
        }

        public void CompleteEnemyTurn()
        {
            if (!EnsurePhase(BattlePhase.EnemyTurn))
            {
                return;
            }
            State.CirclePhase = BattlePhase.EndCircle;
            StateProceed();
        }

        public void RequestAction(BattleAction action)
        {
            if (!EnsurePhase(BattlePhase.PlayerTurn) || action is null)
            {   
                return;
            }
            switch (action)
            {
                case NormalAction nor:
                    NormalActionSelected?.Invoke(nor);
                    break;

                case SkillAction skl:
                    SkillActionSelected?.Invoke(skl);
                    break;

                case SpecialAction spl:
                    State.CirclePhase = BattlePhase.EnemyTurn;
                    SpecialActionSelected?.Invoke(spl);
                    PlayerTurnEnded?.Invoke();
                    StateProceed();
                    break;

                default:
                    throw new ArgumentException($"Unsupported action type: {action.GetType().Name}", nameof(action));
            }
        }

        private bool EnsurePhase(BattlePhase phase)
        {
            if (State.CirclePhase == phase) { 
                return true; 
            }
            return false;
        }

        private void StateProceed()
        {
            switch (State.CirclePhase)
            {
                case BattlePhase.StartCircle:
                    State.CircleIndex += 1;
                    State.CirclePhase = BattlePhase.PlayerTurn;
                    PlayerTurnStarted?.Invoke();
                    break;

                case BattlePhase.EnemyTurn:
                    EnemyTurnStarted?.Invoke();
                    break;

                case BattlePhase.EndCircle:
                    State.CirclePhase = BattlePhase.StartCircle;
                    StateProceed();
                    break;
            } 
        }
    }
}
