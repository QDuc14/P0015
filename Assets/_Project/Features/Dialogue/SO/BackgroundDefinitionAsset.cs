using UnityEngine;

namespace Project.Features.Dialogue.SO
{
    [CreateAssetMenu(menuName = "Project/Dialogue/Background Definition")]
    public sealed class BackgroundDefinitionAsset : ScriptableObject
    {
        public string BackgroundId = "bg_town";
        public Sprite Sprite;
    }
}