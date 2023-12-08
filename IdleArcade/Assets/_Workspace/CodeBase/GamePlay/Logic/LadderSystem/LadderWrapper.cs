using System.Threading.Tasks;
using _Workspace.CodeBase.Extensions;
using _Workspace.CodeBase.GamePlay.Assets;
using _Workspace.CodeBase.GamePlay.Logic.DirtSystem.Handlers;
using _Workspace.CodeBase.Service.Factory;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Workspace.CodeBase.GamePlay.Logic.LadderSystem
{
    public class LadderWrapper : INextLayerHandler
    {
        private const float LadderHeightConst = 0.5f;

        private readonly IPrefabFactoryAsync _prefabFactory;
        private Ladder _ladder;
        private Transform _ladderTransform;

        public LadderWrapper(IPrefabFactoryAsync prefabFactory)
            => _prefabFactory = prefabFactory;

        public async UniTask Initialize()
        {
            _ladder = await _prefabFactory.Create<Ladder>(GamePlayAssetsAddress.Ladder);
            _ladderTransform = _ladder.transform;
        }

        public async void HandleNextLayer(int depth)
        {
            if (!IsValidDepth(depth))
                return;

            await NewLadderPart(depth);
        }

        private async Task NewLadderPart(int depth)
        {
            LadderPart ladderPart = await CreateLadderPart(depth);
            _ladder.AddPart(ladderPart);
        }

        private bool IsValidDepth(int depth)
            => depth % 2 == 1 || depth == 0 ;

        private async Task<LadderPart> CreateLadderPart(int depth)
        {
            LadderPart ladderPart = await _prefabFactory.Create<LadderPart>(GamePlayAssetsAddress.LadderPart,
                _ladderTransform.position.AddY(-depth * LadderHeightConst)
                , _ladderTransform);
            return ladderPart;
        }
    }
}