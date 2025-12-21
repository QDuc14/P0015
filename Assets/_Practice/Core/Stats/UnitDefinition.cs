namespace Practice.Core.Stats
{
    public class UnitDefinition
    {
        public string Id { get; }
        public string DisplayName { get; }
        public int MaxHp { get; }
        public int Attack { get; }
        public int MaxMp { get; }

        public UnitDefinition(string id, string displayName, int maxHp, int attack, int maxMp)
        {
            Id = id;
            DisplayName = displayName;
            MaxHp = maxHp;
            Attack = attack;
            MaxMp = maxMp;
        }
    }
}
