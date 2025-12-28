using Practice.Core;
using UnityEngine;

namespace Practice.Features.Battle.Unity
{
    [CreateAssetMenu(fileName = "GameBalance", menuName = "Practice/Battle/Game Balance")]
    public sealed class GameBalanceAsset : ScriptableObject
    {
        [Header("Critical Hit Settings")]
        [Range(0f, 1f)]
        [SerializeField] private float _criticalHitChance = 0.05f;
        [Min(1f)]
        [SerializeField] private float _criticalHitMultiplier = 1.5f;

        [Header("Damage")]
        [Min(0f)]
        [SerializeField] private float _damageMultiplier = 1.0f;

        [Header("Exp")]
        [Min(0f)]
        [SerializeField] private float _expModifier = 1.0f;

        public GameBalance ToGameBalance()
        {
            return new GameBalance(
                criticalHitChance: _criticalHitChance,
                criticalHitMultiplier: _criticalHitMultiplier,
                globalDamageModifier: _damageMultiplier,
                expModifier: _expModifier
            );
        }
    }
}
