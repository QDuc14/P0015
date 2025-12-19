using Practice.Core.Stats;
using System.Numerics;

namespace Practice.Features.ArcadeMove
{
    public sealed class PlayerMovementLogic
    {
        private readonly PlayerStats _stats;

        public PlayerMovementLogic(PlayerStats stats)
        {
            _stats = stats;
        }

        public Vector2 GetDelta(Vector2 input, float deltaTime)
        {
            // Validate first If there is no input, no movement.
            if (input == Vector2.Zero || deltaTime <= 0f || _stats.MoveSpeed <= 0f)
                return Vector2.Zero;

            // Normalize if magnitude > 1 so diagonal isn't faster.
            if (input.LengthSquared() > 1f)
                input = Vector2.Normalize(input);

            float moveDistance = _stats.MoveSpeed * deltaTime;

            return input * moveDistance;
        }
    }
}
