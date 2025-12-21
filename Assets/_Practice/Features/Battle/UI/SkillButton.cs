using Practice.Core;
using Practice.Features.Battle.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Practice.Features.Battle.UI
{
    public sealed class SkillButton : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _label;

        [Header("Skill")]
        [SerializeField] private SkillDefinitionAsset _skillAsset;

        [Header("Unit")]
        [SerializeField] private BattleUnitView _caster;
        [SerializeField] private BattleUnitView _target;

        private SkillLogic _skillLogic;
        private SKillDefinition _skillDefinition;

        private void Awake()
        {
            _skillLogic = new SkillLogic();
            if (_skillAsset != null)
            {
                _skillDefinition = _skillAsset.ToSkillDefinition();
            }
            else
            {
                Debug.LogError("SkillDefinitionAsset is not assigned.", this);
            }
        }

        private void OnEnable()
        {
            if (_button != null)
            {
                _button.onClick.AddListener(OnClick);
            }

            if (_label != null && _skillAsset != null)
            {
                _label.text = _skillDefinition.DisplayName;
            }
        }

        private void Update()
        {
            if (_button == null || _caster == null || _target == null || _skillDefinition == null)
            {
                return;
            }

            BattleUnit caster = _caster.BattleUnit;
            BattleUnit target = _target.BattleUnit;


            bool canUse = _skillLogic.CanUseSkill(_skillDefinition, caster);

            if (canUse && !target.IsDead)
            {
                _button.interactable = true;
            }
            else
            {
                _button.interactable = false;
            }
        }

        private void OnDisable()
        {
            if (_button != null)
            {
                _button.onClick.RemoveListener(OnClick);
            }
        }

        private void OnClick()
        {
            if (_caster == null || _target == null || _skillDefinition == null)
            {
                return;
            }

            BattleUnit caster = _caster.BattleUnit;
            BattleUnit target = _target.BattleUnit;

            if (!_skillLogic.CanUseSkill(_skillDefinition, caster))
            {
                return;
            }

            if (caster == null || target == null)
            {
                return;
            }

            int mpCost = _skillLogic.CalMpCost(_skillDefinition, caster);
            int damage = _skillLogic.CalDamge(_skillDefinition, caster, target);

            target.TakeDamage(damage);
            caster.ManaDrain(mpCost);

            Debug.Log($"{caster.Name} used {_skillDefinition.DisplayName} on {target.Name}, dealing {damage} damage and costing {mpCost} MP.");
        }
    }
}

 
