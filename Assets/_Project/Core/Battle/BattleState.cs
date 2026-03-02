namespace Project.Core.Battle
{
    public enum BattlePhase
    {
        StartCircle,
        StartTurn,
        SelectCharacter,
        SelectAction,
        EnemyProceed,
        EndTurn,
        EndCircle,
        EndBattle,
    }

    public sealed class BattleState
    {
        public int CircleIndex { get; internal set; } = 0;
        public BattlePhase CirclePhase { get; internal set; }
        
    }
}
