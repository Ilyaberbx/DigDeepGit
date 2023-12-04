using System.Threading.Tasks;
using _Workspace.CodeBase.Infrastructure.Service.Assets;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Workspace.CodeBase.Service.Factory
{
    public class PrefabFactoryAsync : IPrefabFactoryAsync
    {
        private readonly IInstantiator _instantiator;
        private readonly IAssetsProvider _assets;

        public PrefabFactoryAsync(IInstantiator instantiator, IAssetsProvider assets)
        {
            _instantiator = instantiator;
            _assets = assets;
        }

        public async UniTask<T> Create<T>(string address) where T : Object
        {
            GameObject prefab = await _assets.Load<GameObject>(address);
            GameObject createdObject = _instantiator.InstantiatePrefab(prefab);
            return createdObject.GetComponent<T>();
        }

        public async UniTask<T> Create<T>(string address, Vector3 at, Transform container) where T : Object
        {
            GameObject prefab = await _assets.Load<GameObject>(address);
            GameObject createdObject = _instantiator.InstantiatePrefab(prefab, at, container);
            return createdObject.GetComponent<T>();
        }

        public async Task<T> Create<T>(string address, Transform container) where T : Object
        {
            GameObject prefab = await _assets.Load<GameObject>(address);
            GameObject createdObject = _instantiator.InstantiatePrefab(prefab, Vector3.zero, container);
            return createdObject.GetComponent<T>();
        }
    }
}