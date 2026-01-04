using Project.Core.Dialogue;
using Project.Features.Dialogue.Logic;
using Project.Features.Dialogue.SO;
using Project.Features.Dialogue.View;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Features.Dialogue.Unity
{
    public sealed class DialogueController : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private DialogueDatabaseAsset _database;

        [Header("View")]
        [SerializeField] private DialogueView _view;

        [Header("Audio")]
        [SerializeField] private AudioSource _sfxSource;
        [SerializeField] private AudioSource _bgmSource;

        [Header("Input")]
        [SerializeField] private InputActionReference _continueAction;
        [SerializeField] private InputActionReference _autoToggleAction;
        [SerializeField] private InputActionReference _skipToggleAction;

        private readonly DialogueRunner _runner = new();
        private bool _auto;
        private bool _skip;

        private void OnEnable()
        {
            if (_continueAction) _continueAction.action.performed += OnContinue;
            if (_autoToggleAction) _autoToggleAction.action.performed += OnAutoToggle;
            if (_skipToggleAction) _skipToggleAction.action.performed += OnSkipToggle;

            _continueAction.action.Enable();
            _autoToggleAction.action.Enable();
            _skipToggleAction.action.Enable();

            _runner.OnLine += HandleLine;
            _runner.OnCommand += HandleCommand;
            _runner.OnWait += HandleWait;
            _runner.OnFinished += HandleFinished;
        }

        private void Start()
        {
            StartDialogue("Story_000");
        }

        private void Update()
        {
            _runner.Tick(Time.deltaTime);
        }

        private void OnDisable()
        {
            if (_continueAction) _continueAction.action.performed -= OnContinue;
            if (_autoToggleAction) _autoToggleAction.action.performed -= OnAutoToggle;
            if (_skipToggleAction) _skipToggleAction.action.performed -= OnSkipToggle;

            _runner.OnLine -= HandleLine;
            _runner.OnCommand -= HandleCommand;
            _runner.OnWait -= HandleWait;
            _runner.OnFinished -= HandleFinished;
        }

        public void StartDialogue(string scriptId, int startIndex = 0)
        {
            DialogueScriptAsset script = _database.GetScript(scriptId);
            if (script == null)
            {
                Debug.LogError($"Dialogue script not found: {scriptId}");
                return;
            }

            DialogueSession session = new DialogueSession(new DialogueId(scriptId), script.BuildRuntimeNodes(), startIndex);
            _runner.Begin(session);
        }

        private void HandleFinished()
        {
            Debug.Log("Dialogue finished.");
        }

        private void HandleWait(DialogueWait wait)
        {
            throw new NotImplementedException();
        }

        private void HandleCommand(DialogueCommand command)
        {
            switch (command.CommandType)
            {
                case DialogueCommandType.SetBackground:
                    _view.SetBackground(_database.GetBackground(command.A));
                    break;
                

                case DialogueCommandType.SetPortrait:
                    Sprite sprite = ResolvePortrait(command.A, command.B);
                    _view.SetPortrait(sprite, PortraitSlot.Left); // Default left
                    break;

                case DialogueCommandType.ClearPortrait:
                    _view.ClearPortrait(PortraitSlot.Left);
                    break;

                case DialogueCommandType.PlaySfx:

                    break;

                case DialogueCommandType.PlayBgm:

                    break;

                case DialogueCommandType.StopBgm:

                    break;

            }
        }

        private void HandleLine(DialogueLine line)
        {
            string speakerName = ResolveSpeakerName(line.CharacterId);
            _view.ShowLine(speakerName, line.Text);

            if (!string.IsNullOrEmpty(line.CharacterId))
            {
                Sprite sprite = ResolvePortrait(line.CharacterId, line.CharacterExpressionId);
                _view.SetPortrait(sprite, line.Slot);
            }
        }

        private Sprite ResolvePortrait(string characterId, string characterExpressionId)
        {
            CharacterDefinitionAsset c = _database.GetCharacter(characterId);
            return c != null ? c.GetSprite(characterExpressionId) : null;
        }

        private string ResolveSpeakerName(string characterId)
        {
            CharacterDefinitionAsset c = _database.GetCharacter(characterId);
            return c != null ? c.CharacterId : null;
        }

        private void OnSkipToggle(InputAction.CallbackContext context)
        {
            _skip = !_skip;
            _runner.SetSkip(_skip);
        }

        private void OnAutoToggle(InputAction.CallbackContext context)
        {
            _auto = !_auto;
            _runner.SetAuto(_auto);
        }

        private void OnContinue(InputAction.CallbackContext context)
        {
            if (_view != null && _view.IsTyping)
                _view.CompleteTypingInstantly();
            else
                _runner.Continue();
        }

    }
}
