using System;
using System.Collections.Generic;

namespace Project.Core.Dialogue
{
    public sealed class DialogueMachine
    {
        private readonly IReadOnlyList<DialogueNode> _nodes;
        public DialogueState State { get; } = new DialogueState();

        public event Action<DialogueLine> LineStarted;
        public event Action<DialogueCommand> CommandFired;
        public event Action<DialogueWait> WaitStarted;
        public event Action Finished;

        public DialogueMachine(IReadOnlyList<DialogueNode> nodes, int startedIndex = 0)
        {
            _nodes = nodes ?? Array.Empty<DialogueNode>();
            State.NodeIndex = startedIndex;
        }

        public void Start()
        {
            if (_nodes.Count == 0)
            {
                State.Phase = DialoguePhase.Finished;
                Finished?.Invoke();
                return;
            }

            State.Phase = DialoguePhase.Idle;
            AdvanceToNextBeat();
        }

        public void SetAuto(bool auto)
        {
            State.Auto = auto;
        }

        public void SetSkip(bool skip)
        {
            State.Skip = skip;
        }

        public void Tick(float deltaTime)
        {
            if (State.Phase == DialoguePhase.Finished) return;

            if (State.Phase == DialoguePhase.Waiting && !State.WaitForClick)
            {
                State.WaitRemaining -= deltaTime;
                if (State.WaitRemaining <= 0f)
                {
                    AdvanceToNextBeat();
                }
            }
        }

        public void Continue()
        {
            if (State.Phase == DialoguePhase.Finished) return;

            if (State.Phase == DialoguePhase.Waiting && State.WaitForClick)
            {
                AdvanceToNextBeat();
                return;
            }

            if (State.Phase == DialoguePhase.Presenting)
            {
                AdvanceToNextBeat();
                return;
            }

            AdvanceToNextBeat();
        }

        private void AdvanceToNextBeat()
        {
            while (true)
            {
                if (State.NodeIndex >= _nodes.Count)
                {
                    State.Phase = DialoguePhase.Finished;
                    Finished?.Invoke();
                    return;
                }

                var node = _nodes[State.NodeIndex];
                State.NodeIndex++;

                if (node is DialogueCommandNode cmd)
                {
                    CommandFired?.Invoke(cmd.Command);

                    if (State.Skip) continue;

                    continue;
                }

                if (node is DialogueWaitNode wait)
                {
                    if (State.Skip && !wait.Wait.WaitForClick) continue;

                    State.Phase = DialoguePhase.Waiting;
                    State.WaitRemaining = Math.Max(0f, wait.Wait.Seconds);
                    State.WaitForClick = wait.Wait.WaitForClick;
                    WaitStarted?.Invoke(wait.Wait);
                    return;
                }

                if (node is DialogueLineNode line)
                {
                    State.Phase = DialoguePhase.Presenting;
                    LineStarted?.Invoke(line.Line);
                    return;
                }
            }
        }
    }
}
