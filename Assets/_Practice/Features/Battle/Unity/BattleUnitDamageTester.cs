using UnityEngine;
using UnityEngine.InputSystem;

namespace Practice.Features.Battle.Unity
{
    public sealed class BattleUnitDamageTester : MonoBehaviour
    {
        [SerializeField] private BattleUnitView _unitView;
        [SerializeField] private int _damageAmount = 10;
        [SerializeField] private int _healAmount = 5;

        private void Update()
        {
            if (_unitView == null)
                return;
            
            var keyboard = Keyboard.current;
            if (keyboard == null)
                return;

            if (keyboard.dKey.wasPressedThisFrame)
            {                 
                _unitView.ApplyDamage(_damageAmount);
            }
            
            if (keyboard.hKey.wasPressedThisFrame)
            {
                _unitView.ApplyHeal(_healAmount);
            }
        }
    }
}
