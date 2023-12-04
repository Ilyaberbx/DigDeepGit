using System;
using _Workspace.CodeBase.GamePlay.Input;
using UnityEngine;
using Zenject;

namespace _Workspace.CodeBase.GamePlay.Logic.Player
{
    public class PlayerMover : MonoBehaviour
    {
        public event Action<Vector3> OnMoved;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private float _speed;

        private IInputService _input;

        [Inject]
        public void Construct(IInputService input)
            => _input = input;

        private void Update()
            => Move();

        private void Move()
        {
            Vector3 direction = new Vector3(_input.GetHorizontalInput(), 0, _input.GetVerticalInput())
                .normalized;

            Vector3 movement = CalculateMovement(direction);

            _characterController.Move(movement * (_speed * Time.deltaTime));
            
            OnMoved?.Invoke(direction);
        }
        

        private Vector3 CalculateMovement(Vector3 direction) 
            => Vector3.forward * direction.z + Vector3.right * direction.x;
    }
}