using System;
using _Workspace.CodeBase.GamePlay.Logic.DirtSystem;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace _Workspace.CodeBase.GamePlay.Logic.Player
{
    public class PlayerDigger : MonoBehaviour
    {
        [SerializeField] private int _digCooldown = 2;
        private bool _canDig = true;
        public event Action OnDig;

        private void OnTriggerEnter(Collider other)
        {
            if (!_canDig)
                return;

            if (!IsDirt(other, out Dirt dirt))
                return;

            dirt.Dig();
            OnDig?.Invoke();
        }

        private bool IsDirt(Collider other, out Dirt dirt)
            => other.TryGetComponent(out dirt);
        
    }
}