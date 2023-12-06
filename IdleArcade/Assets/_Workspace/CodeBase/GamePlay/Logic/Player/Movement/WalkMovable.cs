using _Workspace.CodeBase.Extensions;
using _Workspace.CodeBase.GamePlay.Input;
using UnityEngine;

namespace _Workspace.CodeBase.GamePlay.Logic.Player.Movement
{
    public class WalkMovable : IMovable
    {
        private readonly CharacterController _characterController;
        private readonly IInputService _input;
        private readonly float _speed;

        public WalkMovable(CharacterController characterController
            , IInputService input
            , float speed)
        {
            _characterController = characterController;
            _input = input;
            _speed = speed;
        }

        public Vector3 Move()
        {
            Vector3 direction = Vector3.zero
                .WithX(_input.GetHorizontalInput())
                .WithZ(_input.GetVerticalInput())
                .normalized;

            Vector3 movement = CalculateMovement(direction);

            _characterController.Move(movement * (_speed * Time.deltaTime));

            return direction;
        }

        private Vector3 CalculateMovement(Vector3 direction)
            => Vector3.forward * direction.z + Vector3.right * direction.x;
    }
}