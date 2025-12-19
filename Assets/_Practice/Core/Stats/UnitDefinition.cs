namespace Practice.Core.Stats
{
    public class UnitDefinition
    {
        public string Id { get; }
        public string DisplayName { get; }
        public int MaxHp { get; }
        public int Attack { get; }

        public UnitDefinition(string id, string displayName, int maxHp, int attack)
        {
            Id = id;
            DisplayName = displayName;
            MaxHp = maxHp;
            Attack = attack;
        }
    }
}
