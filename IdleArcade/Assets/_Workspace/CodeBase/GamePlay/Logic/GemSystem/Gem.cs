﻿using UnityEngine;

namespace _Workspace.CodeBase.GamePlay.Logic.GemSystem
{
    [RequireComponent(typeof(SphereCollider))]
    public class Gem : MonoBehaviour
    {
        [SerializeField] private SphereCollider _collider;
        [SerializeField] private MeshRenderer _renderer;
        [SerializeField] private Color _unlockedColor;
        [SerializeField] private Color _lockedColor;
        [SerializeField] private Rigidbody _rigidbody;
        
        private void Awake() 
            => _renderer.material.color = _lockedColor;

        public void Unlock()
        {
            _collider.enabled = true;
            _rigidbody.isKinematic = false;
            _renderer.material.color = _unlockedColor;
        }
    }
}