using _Workspace.CodeBase.Extensions;
using _Workspace.CodeBase.GamePlay.Assets;
using _Workspace.CodeBase.GamePlay.Logic.DirtSystem.Handlers;
using _Workspace.CodeBase.GamePlay.Logic.Player.Movement;
using _Workspace.CodeBase.Service.Factory;
using UnityEngine;
using Zenject;

namespace _Workspace.CodeBase.GamePlay.Logic.LadderSystem
{
    public class LadderSystem : MonoBehaviour, INextLayerHandler
    {
        [SerializeField] private BoxCollider _wholeLadderCollider;
        [SerializeField] private float _ladderHeightCoefficient;

        private GravitySystem.Gravity _playerGravity;
        private IPrefabFactoryAsync _prefabFactory;
        private Transform _cachedTransform;
        
        private void Awake()
            => _cachedTransform = transform;

        [Inject]
        public void Construct(IPrefabFactoryAsync prefabFactory)
            => _prefabFactory = prefabFactory;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerMover mover))
                mover.SetMovable<LadderMovable>();

            Debug.Log("Climb");
            if (other.TryGetComponent(out GravitySystem.Gravity gravity))
            {
                _playerGravity = gravity;
                _playerGravity.enabled = false;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out PlayerMover mover))
            {
                mover.SetMovable<WalkMovable>();
                _playerGravity.enabled = true;
            }
            
        }

        public async void HandleNextLayer(int depth)
        {
            Ladder ladder = await _prefabFactory.Create<Ladder>(GamePlayAssetsAddress.Ladder,
                _cachedTransform.position.AddY(-depth * _ladderHeightCoefficient)
                , _cachedTransform);
            _wholeLadderCollider.size += Vector3.zero
                .AddY(-depth * ladder.Size * 2)
                .AddZ(-1);


            _wholeLadderCollider.center += Vector3.zero
                .AddY(-depth * ladder.Size);
        }
    }
}