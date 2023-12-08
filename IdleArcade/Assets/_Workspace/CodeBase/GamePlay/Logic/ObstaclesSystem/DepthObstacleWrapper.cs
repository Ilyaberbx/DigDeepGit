using System.Threading.Tasks;
using _Workspace.CodeBase.Extensions;
using _Workspace.CodeBase.GamePlay.Assets;
using _Workspace.CodeBase.GamePlay.Logic.ObstaclesSystem.Handlers;
using _Workspace.CodeBase.Service.Factory;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Workspace.CodeBase.GamePlay.Logic.ObstaclesSystem
{
    public class DepthObstacleWrapper : ILayerUpdateHandler, ILayerDigHandler
    {
        private readonly IPrefabFactoryAsync _prefabFactory;
        private DepthObstacle _currentObstacle;

        public DepthObstacleWrapper(IPrefabFactoryAsync prefabFactory)
            => _prefabFactory = prefabFactory;

        public async UniTask Initialize() 
            => await CreatePitObstacle();

        private async UniTask CreatePitObstacle() 
            => await _prefabFactory
                .Create<Transform>(GamePlayAssetsAddress.PitObstacle);

        private async UniTask<DepthObstacle> NextObstacle(int depth) =>
            await _prefabFactory
                .Create<DepthObstacle>(GamePlayAssetsAddress.DepthObstacle
                    , Vector3.zero.WithY(-depth / 2f)
                    , null);

        public async void HandLayerInitialize(int depth, Color layerColor)
        {
            _currentObstacle = await NextObstacle(depth);
            _currentObstacle.Initialize(depth, layerColor);
        }

        public void HandleLayerDig() 
            => _currentObstacle.Dig();
    }
}