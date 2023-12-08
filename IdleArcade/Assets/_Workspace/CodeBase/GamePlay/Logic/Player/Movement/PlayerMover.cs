using System;
using System.Collections.Generic;
using _Workspace.CodeBase.GamePlay.Input;
using UnityEngine;
using Zenject;

namespace _Workspace.CodeBase.GamePlay.Logic.Player.Movement
{
    public class PlayerMover : MonoBehaviour
    {
        public event Action<Vector3> OnMove;

        [SerializeField] private CharacterController _characterController;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _climbSpeed;
        
        private readonly Dictionary<Type, IMovable> _movables = new();

        private IInputService _input;
        private IMovable _currentMovable;

        [Inject]
        public void Construct(IInputService input)
            => _input = input;

        private void Awake()
        {
            _movables.Add(typeof(WalkMovable), new WalkMovable(_characterController, _input, _moveSpeed));
            _movables.Add(typeof(LadderMovable), new LadderMovable(_characterController, _input, _climbSpeed));
            _movables.Add(typeof(NoMovable), new NoMovable());
            SetMovable<WalkMovable>();
        }

        public void Stop()
        {
            SetMovable<NoMovable>();
            _characterController.enabled = false;
        }

        public void Resume<T>() where T : IMovable
        {
            _characterController.enabled = true;
            SetMovable<T>();
        }

        private void SetMovable<T>() where T : IMovable
        {
            if (_movables.TryGetValue(typeof(T), out IMovable movable))
                _currentMovable = movable;
            else
                Debug.LogError("Where is no this type of movement: " + typeof(T));
        }

        private void Update()
        {
           Vector3 direction = _currentMovable?.MoveWithDirection() ?? Vector3.zero;
           OnMove?.Invoke(direction);
        }
    }
}