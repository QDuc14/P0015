using System.Collections.Generic;
using UnityEngine;

namespace Project.Features.Dialogue.SO
{
    [CreateAssetMenu(menuName = "Project/Dialogue/Dialogue Database")]
    public sealed class DialogueDatabaseAsset : ScriptableObject
    {
        public List<DialogueScriptAsset> Scripts = new();
        public List<CharacterDefinitionAsset> Characters = new();
        public List<BackgroundDefinitionAsset> Backgrounds = new();
        public AudioLibraryAsset AudioLibrary;

        public DialogueScriptAsset GetScript(string id)
        {
            foreach (DialogueScriptAsset asset in Scripts)
                if (asset != null && asset.Id == id) return asset;
            return null;
        }

        public CharacterDefinitionAsset GetCharacter(string id)
        {
            foreach(CharacterDefinitionAsset asset in Characters)
                if (asset != null && asset.CharacterId == id) return asset;
            return null;
        }

        public Sprite GetBackground(string id)
        {
            foreach (BackgroundDefinitionAsset asset in Backgrounds)
                if (asset != null && asset.BackgroundId == id) return asset.Sprite;
            return null;
        }
    }
}
