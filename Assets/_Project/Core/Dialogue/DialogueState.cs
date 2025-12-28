namespace Project.Core.Dialogue
{
    public enum DialoguePhase
    {
        Idle,
        Presenting,
        Waiting,
        Finished
    }

    public sealed class DialogueState
    {
        public int NodeIndex { get; internal set; }
        public DialoguePhase Phase { get; internal set; } = DialoguePhase.Idle;
        public bool IsTyping { get; internal set; }
        public float TypeProgress { get; internal set; }
        public float WaitRemaining { get; internal set; }
        public bool WaitForClick { get; internal set; }
        public bool Auto { get; internal set; }
        public bool Skip { get; internal set; }
    }
}
