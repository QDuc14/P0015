namespace Project.Core.Dialogue
{
    public enum DialogueNodeKind
    {
        Line,
        Command,
        Wait
        // choice add later
    }

    public enum PortraitSlot
    {
        Left,
        Center,
        Right
    }

    public enum DialogueCommandType
    {
        SetBackground,
        SetPortrait,
        ClearPortrait,
        PlaySfx,
        PlayBgm,
        StopBgm
    }

    public readonly struct DialogueLine
    {
        public readonly string CharacterId;
        public readonly string CharacterExpressionId;
        public readonly string SpeakerName;
        public readonly string Text;
        public readonly PortraitSlot Slot;

        public DialogueLine(string characterId, string characterExpressionId, string speakerName, string text, PortraitSlot slot)
        {
            CharacterId = characterId;
            CharacterExpressionId = characterExpressionId;
            SpeakerName = speakerName;
            Text = text;
            Slot = slot;
        }
    }

    public readonly struct DialogueCommand
    {
        public readonly DialogueCommandType CommandType;
        public readonly string A;
        public readonly string B;

        public DialogueCommand(DialogueCommandType commandType, string a = "", string b = "")
        {
            CommandType = commandType;
            A = a ?? "";
            B = b ?? "";
        }
    }

    public readonly struct DialogueWait
    {
        public readonly float Seconds;
        public readonly bool WaitForClick;

        public DialogueWait(float seconds, bool waitForClick)
        {
            Seconds = seconds;
            WaitForClick = waitForClick;
        }
    }

    public abstract class DialogueNode
    {
        public abstract DialogueNodeKind Kind { get; }
    }

    public class DialogueLineNode : DialogueNode
    {
        public override DialogueNodeKind Kind { get => DialogueNodeKind.Line; }
        public DialogueLine Line { get; }
        public DialogueLineNode(DialogueLine line)
        {
            Line = line;
        }
    }

    public class DialogueCommandNode : DialogueNode
    {
        public override DialogueNodeKind Kind { get => DialogueNodeKind.Command; }
        public DialogueCommand Command { get; }
        public DialogueCommandNode(DialogueCommand command)
        {
            Command = command;
        }
    }

    public class DialogueWaitNode : DialogueNode
    {
        public override DialogueNodeKind Kind { get => DialogueNodeKind.Wait; }
        public DialogueWait Wait { get; }
        public DialogueWaitNode(DialogueWait wait)
        {
            Wait = wait;
        }
    }
}