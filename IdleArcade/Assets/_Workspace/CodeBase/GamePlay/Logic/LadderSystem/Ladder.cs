using System.Threading.Tasks;
using _Workspace.CodeBase.Extensions;
using _Workspace.CodeBase.GamePlay.Assets;
using _Workspace.CodeBase.GamePlay.Logic.DirtSystem.Handlers;
using _Workspace.CodeBase.Service.Factory;
using UnityEngine;
using Zenject;

namespace _Workspace.CodeBase.GamePlay.Logic.LadderSystem
{
    public class Ladder : MonoBehaviour, INextLayerHandler
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

        public async void HandleNextLayer(int depth)
        {

            if (depth == 1)
                _prefabFactory.Create<Transform>(GamePlayAssetsAddress.PitObstacle);

            if (depth % 2 == 1)
                return;

            LadderPart ladderPart = await CreateLadderPart(depth);

            AddPart(ladderPart);
        }

        private async Task<LadderPart> CreateLadderPart(int depth)
        {
            LadderPart ladderPart = await _prefabFactory.Create<LadderPart>(GamePlayAssetsAddress.Ladder,
                _cachedTransform.position.AddY(-depth * _ladderHeightCoefficient)
                , _cachedTransform);
            return ladderPart;
        }

        private void AddPart(LadderPart ladderPart)
        {
            _wholeLadderCollider.size += Vector3.zero
                .AddY(ladderPart.Size * 2);

            _wholeLadderCollider.center += Vector3.zero
                .AddY(-ladderPart.Size);
        }
    }
}