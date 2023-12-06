using UnityEngine;

namespace _Workspace.CodeBase.GamePlay.Logic.Camera
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private float _smoothing;
        [SerializeField] private Vector3 _viewOffSet;
        
        
        private Vector3 _currentVelocity = Vector3.zero;

        private Transform _target;

        public void SetTarget(Transform target) 
            => _target = target;

        private void LateUpdate()
        {
            if (_target == null)
                return;

            Vector3 targetPosition = _target.position + _viewOffSet;

            transform.position = Vector3.SmoothDamp(transform.position
                , targetPosition 
                , ref _currentVelocity
                , _smoothing);
        }
    }
}