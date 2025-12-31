using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Features.Dialogue.SO
{
    [CreateAssetMenu(menuName = "Project/Dialogue/Audio Library")]
    public sealed class AudioLibraryAsset : ScriptableObject
    {
        public List<AudioEntry> Sfx = new();
        public List<AudioEntry> Bgm = new();

        [Serializable]
        public struct AudioEntry
        {
            public string Id;
            public AudioClip Clip;
        }

        public AudioClip GetSfx(String id) => Find(Sfx, id);
        public AudioClip GetBgm(String id) => Find(Bgm, id);

        private static AudioClip Find(List<AudioEntry> list, String id)
        {
            foreach (AudioEntry e in list)
                if (e.Id == id) return e.Clip;
            return null;
        }
    }
}
