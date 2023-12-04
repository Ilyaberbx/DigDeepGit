using System;
using UnityEngine;

namespace _Workspace.CodeBase.GamePlay.Logic.Player.Animator
{
    public class PlayerAnimator : MonoBehaviour
    {
        private static readonly int SpeedKey = UnityEngine.Animator.StringToHash("Speed");
        private static readonly int MineKey = UnityEngine.Animator.StringToHash("Mine");

        [SerializeField] private UnityEngine.Animator _animator;
        [SerializeField] private PlayerMover _mover;


        private void Awake() 
            => _mover.OnMoved += PlayMove;

        private void OnDestroy() 
            => _mover.OnMoved -= PlayMove;

        private void Update()
        {
            if(UnityEngine.Input.GetKeyDown(KeyCode.K))
                PlayMine();
        }

        private void PlayMine() 
            => _animator.SetTrigger(MineKey);

        private void PlayMove(Vector3 movement) 
            => _animator.SetFloat(SpeedKey,movement.magnitude);
    }
}