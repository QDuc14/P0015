using Project.Core.Dialogue;
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

        public bool IsTyping { get; private set; }

        public void ShowLine(string speakerName, string text)
        {
            _speakerText.text = speakerName;
            _bodyText.text = text;
            IsTyping = false; // still no typing yet
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

        // if typing implement
        public void CompleteTypingInstantly()
        {
            IsTyping = false;
        }

        public void SetBackground(Sprite sprite)
        {
            if (_background != null)
            {
                _background.sprite = sprite;
            }
        }

    }
}
