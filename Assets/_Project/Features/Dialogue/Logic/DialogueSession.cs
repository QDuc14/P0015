using Project.Core.Dialogue;
using System;
using System.Collections.Generic;

namespace Project.Features.Dialogue.Logic
{
    public sealed class DialogueSession
    {
        public DialogueId Id { get; }
        public IReadOnlyList<DialogueNode> Nodes { get; }
        public int StartIndex { get; }

        public DialogueSession(DialogueId id, IReadOnlyList<DialogueNode> nodes, int startIndex)
        {
            Id = id;
            Nodes = nodes;
            StartIndex = startIndex;
        }
    }
}
