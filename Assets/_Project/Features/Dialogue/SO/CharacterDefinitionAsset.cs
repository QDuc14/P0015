using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Features.Dialogue.SO
{
    [CreateAssetMenu(menuName = "Project/Dialogue/Character Definition")]
    public sealed class CharacterDefinitionAsset : ScriptableObject
    {
        public string CharacterId = "hero";
        public string DisplayName = "Hero";

        public List<ExpressionSprite> expressions = new();

        [Serializable]
        public struct ExpressionSprite
        {
            public string ExpressionId;
            public Sprite Sprite;
        }

        public Sprite GetSprite(string expressionId)
        {
            foreach (var e in expressions)
                if (e.ExpressionId == expressionId) return e.Sprite;
            return null;
        }
    }
}