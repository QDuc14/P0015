using Practice.Core;
using UnityEngine;

namespace Practice.Features.Battle.Unity
{
    [CreateAssetMenu(fileName = "SkillDefinition", menuName = "Practice/Battle/Skill Definition")]
    public sealed class SkillDefinitionAsset : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] private string _id = "Skill ID";
        [SerializeField] private string _displayName = "Skill Name";

        [Header("TextArea")]
        [SerializeField] private string _description = "Skill Description";

        [Header("Specs")]
        [SerializeField] private int _cooldown = 3;
        [SerializeField] private int _power = 50;
        [SerializeField] private int _cost = 10;

        public SKillDefinition ToSkillDefinition()
        {
            return new SKillDefinition(
                id: _id,
                displayName: _displayName,
                discription: _description,
                cooldown: _cooldown,
                mpCost: _cost,
                power: _power
            );
        }
    }
}
