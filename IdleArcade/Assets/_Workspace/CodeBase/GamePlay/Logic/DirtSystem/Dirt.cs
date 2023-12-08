using System;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private GameObject _fullDirtPart;
        [SerializeField] private Collider _gemsCollision;
        [SerializeField] private Renderer[] _renderers;

        [SerializeField] private GameObject _partsRoot;
        
        private IEnumerable<DirtPart> _dirtParts;
        private readonly List<Gem> _gems = new();
        private readonly float _gemsHeightOffSet = 0.25f;

        private IPrefabFactoryAsync _prefabFactory;

        [Inject]
        public void Construct(IPrefabFactoryAsync prefabFactory)
            => _prefabFactory = prefabFactory;

        private void Awake()
        {
            _dirtParts = GetComponentsInChildren<DirtPart>();
            
            foreach (DirtPart part in _dirtParts) 
                part.Initialize();
        }

        public void Initialize(Color color)
        {
            _gems.Clear();
            UpdateColor(color);

            _fullDirtPart.SetActive(true);

            ToggleParts(false);
            ToggleCollision(true);
            
            _collider.isTrigger = false;
        }

        private void UpdateColor(Color color)
        {
            foreach (Renderer renderer in _renderers)
                renderer.material.color = color;
        }

        private void ToggleParts(bool value) 
            => _partsRoot.gameObject.SetActive(value);

        public void Unlock()
            => _collider.isTrigger = true;

        public async void Dig()
        {
            ToggleCollision(false);

            foreach (Gem gem in _gems)
                gem.Unlock();

            _fullDirtPart.SetActive(false);

            ToggleParts(true);

            OnDig?.Invoke();
            
            await ShowParticles();
            
            ToggleParts(false);
        }

        private void ToggleCollision(bool value)
        {
            _collider.enabled = value;
            _gemsCollision.enabled = value;
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

            _gems.AddRange(await UniTask.WhenAll(tasks));
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