namespace Practice.Core
{
    public class SKillDefinition
    {
        public string Id { get; }
        public string DisplayName { get; }
        public string Description { get;}
        public int MpCost { get; set; }
        public int Cooldown { get; }
        public int Power { get; set; }
        public SKillDefinition(string id, string displayName, string discription, int cooldown, int mpCost, int power)
        {
            Id = id;
            DisplayName = displayName;
            Description = discription;
            Cooldown = cooldown;
            MpCost = mpCost;
            Power = power;
        }
    }
}
