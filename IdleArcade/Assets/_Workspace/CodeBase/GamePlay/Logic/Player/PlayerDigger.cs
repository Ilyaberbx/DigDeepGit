using System;
using _Workspace.CodeBase.GamePlay.Logic.DirtSystem;
using UnityEngine;

namespace _Workspace.CodeBase.GamePlay.Logic.Player
{
    public class PlayerDigger : MonoBehaviour
    {
        public event Action OnDig;
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Dirt dirt))
             return;
            
            dirt.Dig();
            OnDig?.Invoke();
        }
    }
}