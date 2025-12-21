using Practice.Core.Stats;
using UnityEngine;

namespace Practice.Features.Battle.Unity
{
    public sealed class BattleUnitView : MonoBehaviour
    {
        [Header("Definition")]
        [SerializeField] private UnitDefinitionAsset _unitDefinitionAsset;

        public BattleUnit BattleUnit { get; private set; }

        private void Awake()
        {
            if (_unitDefinitionAsset == null)
            {
                Debug.LogError("UnitDefinitionAsset is not assigned.", this);
                return;
            }

            UnitDefinition def = _unitDefinitionAsset.ToUnitDefinition();
            BattleUnit = new BattleUnit(def);

            Debug.Log($"BattleUnit '{BattleUnit.Name}' created with {BattleUnit.CurrentHp}/{BattleUnit.MaxHp} HP and {BattleUnit.Attack} Attack.");
        }

        public void ApplyDamage(int amount)
        {
            if (BattleUnit == null)
            {
                Debug.LogError("BattleUnit is not initialized.", this);
                return;
            }

            BattleUnit.TakeDamage(amount);

            Debug.Log($"BattleUnit '{BattleUnit.Name}' took {amount} damage. Current HP: {BattleUnit.CurrentHp}/{BattleUnit.MaxHp}");
        }

        public void ApplyHeal(int amount)
        {
            if (BattleUnit == null)
            {
                Debug.LogError("BattleUnit is not initialized.", this);
                return;
            }

            BattleUnit.Heal(amount);

            Debug.Log($"BattleUnit '{BattleUnit.Name}' healed {amount} HP. Current HP: {BattleUnit.CurrentHp}/{BattleUnit.MaxHp}");
        }

        public void ApplyManaDrain(int amount)
        {
            if (BattleUnit == null)
            {
                Debug.LogError("BattleUnit is not initialized.", this);
                return;
            }
            BattleUnit.ManaDrain(amount);
            Debug.Log($"BattleUnit '{BattleUnit.Name}' drained {amount} MP. Current MP: {BattleUnit.CurrentMp}/{BattleUnit.MaxMp}");
        }
    }
}
