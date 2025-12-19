using Practice.Core.Stats;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityVector2 = UnityEngine.Vector2;
using SystemVector2 = System.Numerics.Vector2;

namespace Practice.Features.ArcadeMove.Unity
{
    public class PlayerMover : MonoBehaviour
    {
        [Header("Stats")]
        [SerializeField] private float _moveSpeed = 5f;

        [Header("Input")]
        [SerializeField] private InputActionReference _moveAction;

        public PlayerStats Stats { get; private set; }
        private PlayerMovementLogic _movementLogic;

        private void Awake()
        {
            Stats = new PlayerStats(_moveSpeed);
            _movementLogic = new PlayerMovementLogic(Stats);
        }

        private void OnEnable()
        {
            _moveAction?.action.Enable();
        }

        private void OnDisable()
        {
            _moveAction?.action.Disable();
        }

        private void Update()
        {
            if (_moveAction == null || _movementLogic == null)
                return;

            UnityVector2 input = _moveAction.action.ReadValue<UnityVector2>();

            SystemVector2 sysInput = new(input.x, input.y);

            SystemVector2 delta = _movementLogic.GetDelta(sysInput, Time.deltaTime);

            UnityVector2 pos = transform.position;
            pos.x += delta.X;
            pos.y += delta.Y;
            transform.position = pos;
        }
    }
}

