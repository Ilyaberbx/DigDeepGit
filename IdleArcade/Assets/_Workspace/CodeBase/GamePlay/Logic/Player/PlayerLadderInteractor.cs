using System.Collections;
using System.Collections.Generic;
using _Workspace.CodeBase.Extensions;
using _Workspace.CodeBase.GamePlay.Input;
using _Workspace.CodeBase.GamePlay.Logic.Player.Movement;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace _Workspace.CodeBase.GamePlay.Logic.Player
{
    public class PlayerLadderInteractor : MonoBehaviour
    {
        [SerializeField] private PlayerMover _mover;
        [SerializeField] private GravitySystem.Gravity _gravity;

        private readonly List<UniTask> _tasks = new();
        [SerializeField] private bool _isOnLadder;
        [SerializeField] private bool _isClimbing;
        private IInputService _input;
        private Coroutine _ladderRoutine;

        [Inject]
        public void Construct(IInputService input)
            => _input = input;

        private async void OnTriggerEnter(Collider other)
        {
            if (_isClimbing)
                return;

            if (other.TryGetComponent(out LadderSystem.Ladder ladder))
            {
                await OnLadderRoutine(ladder);
            }
        }

        private async UniTask OnLadderRoutine(LadderSystem.Ladder ladder)
        {
            _gravity.enabled = false;
            _mover.Stop();

            _isOnLadder = false;

            _tasks.Add(_mover.transform.DOMoveZ(ladder.transform.position.z - 0.5f, 0.3f).ToUniTask());
            _tasks.Add(_mover.transform.DOMoveX(ladder.transform.position.x, 0.2f).ToUniTask());

            Debug.Log("On ladder");

            await UniTask.WhenAny(_tasks);
            
            await _mover.transform.DOLookAt(ladder.transform.position, 0.2f, AxisConstraint.Y).ToUniTask();
            _isOnLadder = true;
            _tasks.Clear();
            _mover.Resume<LadderMovable>();

            _ladderRoutine = null;
        }

        private async void OnTriggerStay(Collider other)
        {
            if (_ladderRoutine != null)
                return;

            if (!_isOnLadder)
                return;

            if (other.TryGetComponent(out LadderSystem.Ladder ladder))
            {
                if (_gravity.TryCatchGround())
                {
                    if (Mathf.Abs(_input.GetHorizontalInput()) < 0.1f)
                        return;

                    if (_input.GetVerticalInput() > -0.1f)
                        return;

                    Debug.Log("ladder ground");
                    _isClimbing = true;
                    _mover.Stop();
                    _isOnLadder = false;
                    _tasks.Clear();
                    _mover.Resume<WalkMovable>();
                    _gravity.enabled = true;
                    _isClimbing = false;
                }
            }
        }

        private async void OnTriggerExit(Collider other)
        {
            if (_isClimbing || !_isOnLadder)
                return;

            if (other.TryGetComponent(out LadderSystem.Ladder ladder))
            {
                _mover.Stop();
                Debug.Log("Leave the ladder");
                await _mover.transform.DOJump(_mover.transform.position
                            .AddZ(2)
                            .WithY(1)
                        , 0.5f
                        , 1
                        , 0.3f)
                    .ToUniTask();

                _isOnLadder = false;
                _isClimbing = false;
                _mover.Resume<WalkMovable>();
                _gravity.enabled = true;
            }
        }
    }
}