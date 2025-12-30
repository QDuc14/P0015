using Project.Core.Dialogue;
using System;

namespace Project.Features.Dialogue.Logic
{
    public sealed class DialogueRunner
    {
        public DialogueMachine Machine { get; private set; }

        public event Action<DialogueLine> OnLine;
        public event Action<DialogueCommand> OnCommand;
        public event Action<DialogueWait> OnWait;
        public event Action OnFinished;

        public void Begin(DialogueSession session)
        {
            Machine = new DialogueMachine(session.Nodes, session.StartIndex);

            Machine.LineStarted += l => OnLine?.Invoke(l);
            Machine.WaitStarted += w => OnWait?.Invoke(w);
            Machine.CommandFired += cmd => OnCommand?.Invoke(cmd);
            Machine.Finished += () => OnFinished?.Invoke();

            Machine.Start();
        }

        public void Tick(float dt) => Machine?.Tick(dt);

        public void Continue() => Machine?.Continue();

        public void SetAuto(bool enabled) => Machine?.SetAuto(enabled);

        public void SetSkip(bool enabled) => Machine?.SetSkip(enabled);
    }
}
