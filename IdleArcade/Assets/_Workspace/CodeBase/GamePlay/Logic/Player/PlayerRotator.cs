using UnityEngine;

namespace _Workspace.CodeBase.GamePlay.Logic.Player
{
    public class PlayerRotator : MonoBehaviour
    {
        [SerializeField] private PlayerMover _mover;
        [SerializeField] private float _rotationSpeed;

        private void Awake()
            => _mover.OnMoved += RotateToMovementDirection;

        private void OnDestroy()
            => _mover.OnMoved -= RotateToMovementDirection;

        private void RotateToMovementDirection(Vector3 direction)
        {
            if (direction == Vector3.zero)
                return;

            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            RotateTo(toRotation);
        }

        private void RotateTo(Quaternion toRotation)
            => transform.rotation =
                Quaternion.RotateTowards(transform.rotation, toRotation, _rotationSpeed * Time.deltaTime);
    }
}