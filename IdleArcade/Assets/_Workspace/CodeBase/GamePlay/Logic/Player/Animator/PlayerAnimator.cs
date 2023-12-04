using UnityEngine;

namespace _Workspace.CodeBase.GamePlay.Logic.Player.Animator
{
    public class PlayerAnimator : MonoBehaviour
    {
        private static readonly int SpeedKey = UnityEngine.Animator.StringToHash("Speed");
        private static readonly int MineKey = UnityEngine.Animator.StringToHash("Mine");

        [SerializeField] private UnityEngine.Animator _animator;
        [SerializeField] private PlayerMover _mover;
        [SerializeField] private PlayerDigger _digger;
        
        private void Awake()
        {
            _mover.OnMove += PlayMove;
            _digger.OnDig += PlayDig;
        }

        private void OnDestroy()
        {
            _mover.OnMove -= PlayMove;
            _digger.OnDig -= PlayDig;
        }

        private void PlayDig() 
            => _animator.SetTrigger(MineKey);

        private void PlayMove(Vector3 movement) 
            => _animator.SetFloat(SpeedKey,movement.magnitude);
    }
}