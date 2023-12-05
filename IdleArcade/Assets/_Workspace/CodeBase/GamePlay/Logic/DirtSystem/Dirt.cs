using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _Workspace.CodeBase.Extensions;
using _Workspace.CodeBase.GamePlay.Assets;
using _Workspace.CodeBase.GamePlay.Logic.GemSystem;
using _Workspace.CodeBase.GamePlay.Logic.Gravity;
using _Workspace.CodeBase.Service.Factory;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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
        [SerializeField] private GameObject _fullDirtPart;


        private IEnumerable<DirtPart> _dirtParts;
        private readonly List<Gem> _gems = new();
        private readonly float _gemsHeightOffSet = 0.25f;

        private IPrefabFactoryAsync _prefabFactory;

        [Inject]
        public void Construct(IPrefabFactoryAsync prefabFactory)
            => _prefabFactory = prefabFactory;

        private void Awake()
            => _dirtParts = GetComponentsInChildren<DirtPart>();

        public void Initialize()
        {
            gameObject.SetActive(true);
            _fullDirtPart.SetActive(true);
            _gems.Clear();
            _collider.isTrigger = false;
        }

        public void Unlock()
            => _collider.isTrigger = true;

        public async void Dig()
        {
            foreach (Gem gem in _gems)
                gem.Unlock();

            _fullDirtPart.SetActive(false);

            OnDig?.Invoke();

            await ShowParticles();

            gameObject.SetActive(false);
        }

        private async UniTask ShowParticles()
        {
            IEnumerable<UniTask> tasks = Enumerable
                .Select(_dirtParts, part => part.Dig());

            await UniTask.WhenAll(tasks);
        }

        public void UpdateDepth(int depth)
        {
            Transform cachedTransform = transform;
            cachedTransform.position = cachedTransform.position.WithY(-depth / 2f);
        }

        public async UniTask SpawnGems(int gems)
        {
            List<UniTask<Gem>> tasks = new List<UniTask<Gem>>();

            for (int i = 0; i < gems; i++)
                tasks.Add(SpawnGem());

            _gems
                .AddRange(
                    await UniTask
                        .WhenAll(tasks));
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
    }
}