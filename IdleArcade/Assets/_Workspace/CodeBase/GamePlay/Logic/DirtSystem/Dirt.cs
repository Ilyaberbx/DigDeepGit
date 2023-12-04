using System;
using System.Collections.Generic;
using _Workspace.CodeBase.Extensions;
using _Workspace.CodeBase.GamePlay.Assets;
using _Workspace.CodeBase.GamePlay.Logic.GemSystem;
using _Workspace.CodeBase.GamePlay.Logic.Gravity;
using _Workspace.CodeBase.Service.Factory;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace _Workspace.CodeBase.GamePlay.Logic.DirtSystem
{
    [RequireComponent(typeof(Ground))]
    public class Dirt : MonoBehaviour
    {
        public event Action OnDig;

        [SerializeField] private Collider _collider;
        [SerializeField] private float _gemsSpawnRange;

        private readonly List<Gem> _gems = new();
        private readonly float _gemsHeightOffSet = 0.25f;

        private IPrefabFactoryAsync _prefabFactory;

        [Inject]
        public void Construct(IPrefabFactoryAsync prefabFactory)
            => _prefabFactory = prefabFactory;

        public async UniTask Initialize(int gems)
        {
            gameObject.SetActive(true);

            _gems.Clear();

            List<UniTask<Gem>> tasks = new List<UniTask<Gem>>();

            for (int i = 0; i < gems; i++)
                tasks.Add(SpawnGem());

            _gems
                .AddRange(await UniTask
                    .WhenAll(tasks));

            _collider.isTrigger = false;
        }

        private async UniTask<Gem> SpawnGem()
            => await _prefabFactory.Create<Gem>(
                GamePlayAssetsAddress.Gem
                , transform.position
                    .AddX(RandomPosition())
                    .AddY(_gemsHeightOffSet)
                    .AddZ(RandomPosition())
                , null);

        private float RandomPosition()
            => Random.Range(-_gemsSpawnRange, _gemsSpawnRange);

        public void Unlock()
            => _collider.isTrigger = true;

        public void Dig()
        {
            foreach (Gem gem in _gems)
                gem.Unlock();

            OnDig?.Invoke();
            gameObject.SetActive(false);
        }
    }
}