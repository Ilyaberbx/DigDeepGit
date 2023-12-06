using _Workspace.CodeBase.GamePlay.Logic.Gravity;
using UnityEngine;

namespace _Workspace.CodeBase.GamePlay.Logic.GravitySystem
{
    public class Gravity : MonoBehaviour
    {
        private const float GroundedGravityConst = -2f;
        private const float GravityCoefficientConst = 1.05f;

        [SerializeField] private float _gravity = 120f;
        [SerializeField] private CheckPoint _checkingGroundPoint;
        [SerializeField] private CharacterController _characterController;
        private Vector3 _velocity;

        private void Initialize()
            => _gravity *= GravityCoefficientConst;

        private void Awake() => Initialize();

        private void LateUpdate()
            => CalculateGravity();


        private bool TryCatchGround()
        {
            Collider[] hits = Physics.OverlapSphere(_checkingGroundPoint.Position, _checkingGroundPoint.Radius);

            foreach (Collider check in hits)
            {
                if (check.transform == null) continue;

                Ground ground = check.transform.GetComponentInParent<Ground>();

                if (ground == null) continue;

                return true;
            }

            return false;
        }


        private void CalculateGravity()
        {
            if (TryCatchGround() && _velocity.y < 0)
                _velocity.y = GroundedGravityConst;

            _velocity.y -= _gravity * Time.deltaTime;
            _characterController.Move(_velocity * Time.deltaTime);
        }
    }
}