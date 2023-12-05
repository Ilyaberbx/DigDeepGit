using System.Collections.Generic;
using System.Threading.Tasks;
using _Workspace.CodeBase.GamePlay.Logic.GemSystem;
using _Workspace.CodeBase.Service.Factory;
using Cysharp.Threading.Tasks;

namespace _Workspace.CodeBase.GamePlay.Logic.DirtSystem
{
    public class DirtSystem
    {
        private const int LayersCountConst = 6;
        private const int LayerSizeX = 5;
        private const int LayerSizeY = 4;

        private readonly IPrefabFactoryAsync _prefabFactory;
        private readonly IGemsProvider _gemsProvider;
        private readonly Queue<DirtLayer> _layersQueue = new();
        private DirtLayer _currentLayer;
        private int _depth = 1;

        public DirtSystem(IPrefabFactoryAsync prefabFactory, IGemsProvider gemsProvider)
        {
            _prefabFactory = prefabFactory;
            _gemsProvider = gemsProvider;
        }

        public async UniTask Initialize()
        {
            CreateLayers();
            await InitializeLayers();
            UnlockLayer();
        }

        private void CreateLayers()
        {
            for (int i = 0; i < LayersCountConst; i++)
                _layersQueue.Enqueue(new DirtLayer(_prefabFactory, _gemsProvider));
        }

        private async UniTask InitializeLayers()
        {
            List<UniTask> tasks = new List<UniTask>();
            
            foreach (DirtLayer layer in _layersQueue)
            {
                _depth++;
                layer.OnDig += HandleLayerDig;
                tasks.Add(layer.Initialize(LayerSizeX, LayerSizeY, _depth));
            }
            await UniTask.WhenAll(tasks);
        }

        private void HandleLayerDig()
            => MoveNext().Forget();

        private async UniTask MoveNext()
        {
            _depth++;
            MoveDigLayerToEnd();
            await UnlockLayer();
        }

        private async UniTask UnlockLayer()
        {
            _currentLayer = _layersQueue.Dequeue();
            DirtLayer nextLayer = _layersQueue.Peek();
            
            await _currentLayer.AppearGems(_depth);
            await nextLayer.AppearGems(_depth);
            
            _currentLayer.Unlock();
        }

        private void MoveDigLayerToEnd()
        {
            _currentLayer.Update(_depth);
            _layersQueue.Enqueue(_currentLayer);
        }
    }
}