using Project.Core.Dialogue;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Features.Dialogue.View
{
    public sealed class DialogueView : MonoBehaviour
    {
        [Header("Text")]
        [SerializeField] private TMP_Text _speakerText;
        [SerializeField] private TMP_Text _bodyText;

        [Header("Portraits")]
        [SerializeField] private Image _leftPortrait;
        [SerializeField] private Image _centerPortrait;
        [SerializeField] private Image _rightPortrait;

        [Header("Background")]
        [SerializeField] private Image _background;

        [Header("Typewriter")]
        [SerializeField, Min(1f)] private float _characterPerScnd = 35f;

        public bool IsTyping => _typingRoutine != null;

        public event Action TypewriterFinished;

        private Coroutine _typingRoutine;

        public void ShowLine(string speakerName, string text, bool instant = false)
        {
            _speakerText.text = speakerName;
            StartTypewriter(text ?? "", instant);
        }

        public void CompleteTypingInstantly()
        {
            if (_typingRoutine != null)
            {
                StopCoroutine(_typingRoutine);
                _typingRoutine = null;
            }

            if (!_bodyText)
                return;

            _bodyText.firstVisibleCharacter = int.MaxValue;
            TypewriterFinished.Invoke();
        }

        public void SetPortrait(Sprite sprite, PortraitSlot slot)
        {
            Image img = slot switch
            {
                PortraitSlot.Left => _leftPortrait,
                PortraitSlot.Right => _rightPortrait,
                _ => _centerPortrait
            };

            if (img == null) return;

            img.sprite = sprite;
            img.enabled = sprite != null;
        }

        public void ClearPortrait(PortraitSlot slot)
        {
            SetPortrait(null, slot);
        }

        public void SetBackground(Sprite sprite)
        {
            if (_background != null)
            {
                _background.sprite = sprite;
            }
        }

        private void StartTypewriter(string text, bool instant)
        {
            if (_typingRoutine != null)
            {
                StopCoroutine(_typingRoutine);
                _typingRoutine = null;
            }

            if (!_bodyText)
                return;

            _bodyText.text = text;

            _bodyText.maxVisibleCharacters = 0;
            _bodyText.ForceMeshUpdate();

            if (instant)
            {
                _bodyText.maxVisibleCharacters = int.MaxValue;
                TypewriterFinished?.Invoke();
                return;
            }

            _typingRoutine = StartCoroutine(TypeRoutine());
        }

        private IEnumerator TypeRoutine()
        {
            // characterCount excludes rich text tags.
            int total = _bodyText.textInfo.characterCount;

            if (total <= 0)
            {
                _typingRoutine = null;
                TypewriterFinished.Invoke();
                yield break;
            }

            float visible = 0f;
            while (visible < total)
            {
                visible += _characterPerScnd * Time.deltaTime;
                _bodyText.maxVisibleCharacters = Mathf.Clamp((int)visible, 0, total);
                yield return null;
            }

            _bodyText.maxVisibleCharacters = total;
            _typingRoutine = null;
            TypewriterFinished?.Invoke();
        }
    }
}
