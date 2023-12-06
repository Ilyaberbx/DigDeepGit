using System;
using System.Collections.Generic;
using _Workspace.CodeBase.Extensions;
using _Workspace.CodeBase.GamePlay.Assets;
using _Workspace.CodeBase.GamePlay.Logic.DirtSystem.StaticData;
using _Workspace.CodeBase.GamePlay.Logic.GemSystem;
using _Workspace.CodeBase.Service.Factory;
using Cysharp.Threading.Tasks;
using ModestTree;
using UnityEngine;

namespace _Workspace.CodeBase.GamePlay.Logic.DirtSystem
{
    public class DirtLayer
    {
        public event Action OnDig;

        private readonly IPrefabFactoryAsync _prefabFactory;
        private readonly IGemsProvider _gemsProvider;
        private readonly DirtConfig _dirtConfig;
        private readonly List<Dirt> _dirtList = new();

        private int _digDirtCount;
        private DepthObstacle _obstacle;

        public DirtLayer(IPrefabFactoryAsync prefabFactory
            , IGemsProvider gemsProvider
            , DirtConfig dirtConfig)
        {
            _prefabFactory = prefabFactory;
            _gemsProvider = gemsProvider;
            _dirtConfig = dirtConfig;
        }

        public async UniTask Initialize(int sizeX, int sizeY, int depth)
        {
            await CreateDirtField(sizeX, sizeY);
            Update(depth);
        }

        private async UniTask<DepthObstacle> CreateDepthObstacle(int depth) =>
            await _prefabFactory
                .Create<DepthObstacle>(GamePlayAssetsAddress.DepthObstacle
                    , Vector3.zero
                        .WithY(-depth / 2f),
                    null);

        public async UniTask AppearGems(int depth)
        {
            Dictionary<Dirt, int> gemsMap = _gemsProvider.GetGemsByDirtMap(_dirtList, depth);

            foreach (Dirt dirt in gemsMap.Keys)
                await dirt.SpawnGems(gemsMap[dirt]);
        }

        private void UpdateDirtField(int depth)
        {
            foreach (Dirt dirt in _dirtList)
            {
                dirt.UpdateDepth(depth);
                dirt.Initialize(GetLayerColor(depth));
            }
        }

        private Color GetLayerColor(int depth)
            => depth % 2 == 0 ? _dirtConfig.OddColor : _dirtConfig.EvenColor;

        private async UniTask CreateDirtField(int sizeX, int sizeY)
        {
            for (int i = 0; i < sizeY; i++)
            {
                for (int j = 0; j < sizeX; j++)
                {
                    Dirt dirt = await CreateDirt(i * 2, j * 2);
                    dirt.OnDig += HandleDirtDig;
                    _dirtList.Add(dirt);
                }
            }
        }

        public async void Update(int depth)
        {
            if (_dirtList.IsEmpty())
                return;

            Reset();
            _obstacle = await CreateDepthObstacle(depth);
            _obstacle.Initialize(depth, GetLayerColor(depth));
            UpdateDirtField(depth);
        }

        public void Unlock()
        {
            foreach (Dirt dirt in _dirtList)
                dirt.Unlock();
        }

        private void Reset()
            => _digDirtCount = 0;

        private void HandleDirtDig()
        {
            _digDirtCount++;

            if (_digDirtCount >= _dirtList.Count)
            {
                _obstacle.Dig();
                OnDig?.Invoke();
            }
        }


        private async UniTask<Dirt> CreateDirt(float x, float z) =>
            await _prefabFactory
                .Create<Dirt>(GamePlayAssetsAddress.Dirt, Vector3.zero
                        .AddX(x)
                        .AddZ(z),
                    null);
    }
}