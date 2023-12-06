using _Workspace.CodeBase.Extensions;
using _Workspace.CodeBase.GamePlay.Input;
using UnityEngine;

namespace _Workspace.CodeBase.GamePlay.Logic.Player.Movement
{
    public class LadderMovable : IMovable
    {
        private readonly CharacterController _characterController;
        private readonly IInputService _input;
        private readonly float _speed;

        public LadderMovable(CharacterController characterController
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
                .WithY(_input.GetVerticalInput())
                .normalized;

            Vector3 movement = CalculateMovement(direction);

            _characterController.Move(movement * (_speed * Time.deltaTime));

            return Vector3.zero;
        }

        private Vector3 CalculateMovement(Vector3 direction) 
            => Vector3.up * direction.y;
    }
}