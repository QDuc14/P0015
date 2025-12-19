using Practice.Core.Stats;
using UnityEngine;

namespace Practice.Features.Battle.Unity
{
    [CreateAssetMenu(fileName = "UnitDefinition", menuName = "Practice/Battle/Unit Definition")]
    public sealed class UnitDefinitionAsset : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] private string _id = "unit_id";
        [SerializeField] private string _displayName = "Unit Name";

        [Header("Stats")]
        [SerializeField] private int _maxHp = 100;
        [SerializeField] private int _attack = 10;

        public UnitDefinition ToUnitDefinition()
        {
            return new UnitDefinition(
                id: _id,
                displayName: _displayName,
                maxHp: _maxHp,
                attack: _attack
            );
        }
    }
}
