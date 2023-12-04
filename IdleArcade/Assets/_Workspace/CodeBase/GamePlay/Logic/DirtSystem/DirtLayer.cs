using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Workspace.CodeBase.Extensions;
using _Workspace.CodeBase.GamePlay.Assets;
using _Workspace.CodeBase.Service.Factory;
using Cysharp.Threading.Tasks;
using ModestTree;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Workspace.CodeBase.GamePlay.Logic.DirtSystem
{
    public class DirtLayer
    {
        public event Action OnDig;

        private const int MinGems = 20;
        private const int MaxGems = 40;

        private readonly IPrefabFactoryAsync _prefabFactory;
        private readonly List<Dirt> _dirtList = new();

        private int _gemsCount;
        private int _digDirtCount;

        public DirtLayer(IPrefabFactoryAsync prefabFactory)
            => _prefabFactory = prefabFactory;

        public async UniTask Initialize(int sizeX, int sizeY, int depth)
        {
            Reset(depth);

            List<UniTask> tasks = new List<UniTask>();

            for (int i = 0; i < sizeY; i++)
            {
                for (int j = 0; j < sizeX; j++)
                {
                    Dirt dirt = await CreateDirtLayer(i * 2, -depth / 2f, j * 2);
                    dirt.OnDig += HandleDirtDig;

                    tasks.Add(dirt.Initialize(GetRandomGamesCount()));
                    _dirtList.Add(dirt);
                }
            }

            await UniTask.WhenAll(tasks);
        }

        public async UniTask Update(int depth)
        {
            if (_dirtList.IsEmpty())
                return;

            Reset(depth);

            List<UniTask> tasks = new List<UniTask>();

            foreach (Dirt dirtLayer in _dirtList)
            {
                UpdateDirtDepth(depth, dirtLayer);
                tasks.Add(dirtLayer.Initialize(GetRandomGamesCount()));
            }

            await UniTask.WhenAll(tasks);
        }

        public void Unlock()
        {
            foreach (Dirt dirt in _dirtList)
                dirt.Unlock();
        }

        private void Reset(int depth)
        {
            _digDirtCount = 0;
            _gemsCount = CalculateLayerGemsCount(depth);
        }

        private void HandleDirtDig()
        {
            _digDirtCount++;

            if (_digDirtCount >= _dirtList.Count)
                OnDig?.Invoke();
        }

        private int GetRandomGamesCount()
            => _gemsCount / 7;

        private int CalculateLayerGemsCount(int depth)
            => Random.Range(MinGems, MaxGems + 1) + 2 * depth;

        private void UpdateDirtDepth(int depth, Dirt dirt)
        {
            Transform cachedTransform = dirt.transform;
            cachedTransform.position = cachedTransform.position.WithY(-depth / 2f);
        }

        private async Task<Dirt> CreateDirtLayer(float x, float y, float z) =>
            await _prefabFactory
                .Create<Dirt>(GamePlayAssetsAddress.Dirt, Vector3.zero
                        .AddX(x)
                        .AddY(y)
                        .AddZ(z),
                    null);
    }
}