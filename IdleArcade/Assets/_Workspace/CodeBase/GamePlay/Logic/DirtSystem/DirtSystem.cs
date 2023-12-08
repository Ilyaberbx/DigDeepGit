using System.Collections.Generic;
using _Workspace.CodeBase.GamePlay.Logic.DirtSystem.Handlers;
using _Workspace.CodeBase.GamePlay.Logic.DirtSystem.StaticData;
using _Workspace.CodeBase.GamePlay.Logic.GemSystem;
using _Workspace.CodeBase.GamePlay.Logic.ObstaclesSystem.Handlers;
using _Workspace.CodeBase.Service.EventBus;
using _Workspace.CodeBase.Service.Factory;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Workspace.CodeBase.GamePlay.Logic.DirtSystem
{
    public class DirtSystem : ILayerDigHandler
    {
        private const int LayersCountConst = 18;
        private const int LayerSizeX = 5;
        private const int LayerSizeY = 4;

        private readonly IPrefabFactoryAsync _prefabFactory;
        private readonly IGemsProvider _gemsProvider;
        private readonly IEventBusService _eventBus;
        private readonly DirtConfig _dirtConfig;
        private readonly Queue<DirtLayer> _layersQueue = new();
        private DirtLayer _currentLayer;
        private int _depth;

        public DirtSystem(IPrefabFactoryAsync prefabFactory
            , IGemsProvider gemsProvider
            , IEventBusService eventBus
            , DirtConfig dirtConfig)
        {
            _prefabFactory = prefabFactory;
            _gemsProvider = gemsProvider;
            _eventBus = eventBus;
            _dirtConfig = dirtConfig;
        }

        public async UniTask Initialize()
        {
            CreateLayers();
            await InitializeLayers();
            await UnlockLayer();
            InformNextLayer();
        }

        private void CreateLayers()
        {
            for (int i = 0; i < LayersCountConst; i++)
                _layersQueue.Enqueue(NewLayer());
        }

        private DirtLayer NewLayer() =>
            new(_prefabFactory
                , _gemsProvider
                , _eventBus
                ,_dirtConfig);

        private async UniTask InitializeLayers()
        {
            List<UniTask> tasks = new List<UniTask>();

            DirtLayer[] layerArray = _layersQueue.ToArray();

            for (int i = 0; i < layerArray.Length; i++)
            {
                DirtLayer layer = layerArray[i];
                tasks.Add(layer.Initialize(LayerSizeX, LayerSizeY, i));
            }

            await UniTask.WhenAll(tasks);
        }

        public void HandleLayerDig()
        {
            Debug.Log("Next");
            MoveNext().Forget();
        }

        private async UniTask MoveNext()
        {
            _depth++;
            MoveDigLayerToEnd();
            await UnlockLayer();
            InformNextLayer();
        }

        private void InformNextLayer()
            => _eventBus.RaiseEvent<INextLayerHandler>(handler =>
                handler.HandleNextLayer(_depth));

        private async UniTask UnlockLayer()
        {
            _currentLayer = _layersQueue.Dequeue();
            
            DirtLayer nextLayer = _layersQueue.Peek();
            await nextLayer.AppearGems(_depth);

            _currentLayer.Unlock();
        }

        private void MoveDigLayerToEnd()
        {
            _currentLayer.Update(_layersQueue.Count + _depth);
            _layersQueue.Enqueue(_currentLayer);
        }
    }
}