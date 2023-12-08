using System.Collections.Generic;
using _Workspace.CodeBase.Extensions;
using _Workspace.CodeBase.GamePlay.Input;
using _Workspace.CodeBase.GamePlay.Logic.LadderSystem;
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
        private bool _isOnLadder;
        private bool _isRejecting;
        private IInputService _input;
        private Coroutine _ladderRoutine;

        [Inject]
        public void Construct(IInputService input)
            => _input = input;

        private async void OnTriggerEnter(Collider other)
        {
            if (_isRejecting)
                return;

            if (IsLadder(other, out Ladder ladder))
            {
                _gravity.enabled = false;
                _mover.Stop();

                _isOnLadder = false;

                await ApplyLadderPosition(ladder);

                _isOnLadder = true;
                _mover.Resume<LadderMovable>();

                _ladderRoutine = null;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (_ladderRoutine != null)
                return;

            if (!_isOnLadder)
                return;

            if (IsLadder(other))
            {
                if (_gravity.TryCatchGround())
                {
                    if (!EnoughToReject()) 
                        return;

                    RejectFromLadder();
                }
            }
        }

        private async void OnTriggerExit(Collider other)
        {
            if (_isRejecting || !_isOnLadder)
                return;

            if (IsLadder(other))
            {
                _mover.Stop();

                await PushToLadderTop();

                _isOnLadder = false;
                _isRejecting = false;

                _mover.Resume<WalkMovable>();

                _gravity.enabled = true;
            }
        }

        private async UniTask ApplyLadderPosition(Ladder ladder)
        {
            Vector3 ladderPosition = ladder.transform.position;
            
            _tasks.Add(_mover.transform.DOMoveZ(ladderPosition.z - 0.5f, 0.3f).ToUniTask());
            _tasks.Add(_mover.transform.DOMoveX(ladderPosition.x, 0.2f).ToUniTask());
            
            await UniTask.WhenAny(_tasks);
            _tasks.Clear();
            
            await _mover.transform.DOLookAt(ladderPosition, 0.2f, AxisConstraint.Y).ToUniTask();
        }

        private void RejectFromLadder()
        {
            _mover.Stop();

            _isRejecting = true;
            _isOnLadder = false;

            _mover.Resume<WalkMovable>();

            _gravity.enabled = true;
            _isRejecting = false;
        }

        private bool EnoughToReject()
        {
            if (Mathf.Abs(_input.GetHorizontalInput()) < 0.1f)
                return false;

            if (_input.GetVerticalInput() > -0.1f)
                return false;
            
            return true;
        }

        private bool IsLadder(Collider collision, out Ladder ladder)
            => collision.TryGetComponent(out ladder);

        private bool IsLadder(Collider collision)
            => collision.TryGetComponent(out Ladder _);

        private async UniTask PushToLadderTop() =>
            await _mover.transform.DOJump(_mover.transform.position
                .AddZ(2)
                .WithY(1), 0.5f, 1, 0.3f).ToUniTask();
    }
}