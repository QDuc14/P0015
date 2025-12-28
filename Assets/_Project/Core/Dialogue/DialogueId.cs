namespace Project.Core.Dialogue
{
    public readonly struct DialogueId
    {
        public readonly string Id;
        
        public DialogueId(string id)
        {
            Id = id ?? string.Empty;
        }

        public override string ToString()
        {
            return Id;
        }
    }
}
