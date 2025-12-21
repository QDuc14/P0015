using Practice.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Practice.Features.Battle.Unity
{
    public sealed class BattleUnitDamageTester : MonoBehaviour
    {
        [SerializeField] private BattleUnitView _mainUnitView;
        [SerializeField] private BattleUnitView _targetUnitView;
        
        [SerializeField] private int _damageAmount = 10;
        [SerializeField] private int _healAmount = 5;

        private void Update()
        {
            if (_mainUnitView == null)
                return;
            
            var keyboard = Keyboard.current;
            if (keyboard == null)
                return;

            if (keyboard.dKey.wasPressedThisFrame)
            {
                _mainUnitView.ApplyDamage(_damageAmount);
            }
            
            if (keyboard.hKey.wasPressedThisFrame)
            {
                _mainUnitView.ApplyHeal(_healAmount);
            }

        }
    }
}
