namespace Project.Core.Battle
{
    public enum BattlePhase
    {
        StartCircle,
        PlayerTurn,
        EnemyTurn,
        EndCircle,
        EndBattle,
    }


    public sealed class BattleState
    {
        public int CircleIndex { get; internal set; } = 0;
        public BattlePhase CirclePhase { get; internal set; }
    }
}
