using Project.Core.Dialogue;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Features.Dialogue.SO
{
    [CreateAssetMenu(menuName = "Project/Dialogue/Dialogue Script")]
    public class DialogueScriptAsset : ScriptableObject
    {
        public string Id = "Story_000";
        public List<NodeDef> Nodes;

        public IReadOnlyList<DialogueNode> BuildRuntimeNodes()
        {
            var list = new List<DialogueNode>(Nodes.Count);
            foreach (NodeDef n in Nodes)
                list.Add(n.ToRuntimeNode());
            return list;
        }

        [Serializable]
        public abstract class NodeDef
        {
            public abstract DialogueNode ToRuntimeNode();
        }

        [Serializable]
        public sealed class LineNodeDef : NodeDef
        {
            public string CharacterId;
            [TextArea] public string Text;

            public string SpeakerName;
            public string ExpressionId;
            public PortraitSlot Slot = PortraitSlot.Left;

            public override DialogueNode ToRuntimeNode()
                => new DialogueLineNode(new DialogueLine(CharacterId, ExpressionId, SpeakerName, Text, Slot));
        }

        [Serializable]
        public sealed class CommandNodeDef : NodeDef
        {
            public DialogueCommandType CommandType;
            public string A;
            public string B;

            public override DialogueNode ToRuntimeNode()
                => new DialogueCommandNode(new DialogueCommand(CommandType, A, B));
        }

        [Serializable]
        public sealed class WaitNodeDef : NodeDef
        {
            public float Seconds;
            public bool WaitForClick;

            public override DialogueNode ToRuntimeNode()
                => new DialogueWaitNode(new DialogueWait(Seconds, WaitForClick));
        }

    }
}
