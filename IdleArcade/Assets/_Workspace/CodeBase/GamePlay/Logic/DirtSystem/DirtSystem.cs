using System.Collections.Generic;
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
        private readonly Queue<DirtLayer> _layersQueue = new();
        private int _depth = 1;

        public DirtSystem(IPrefabFactoryAsync prefabFactory)
            => _prefabFactory = prefabFactory;

        public async UniTask Initialize()
        {
            for (int i = 0; i < LayersCountConst; i++)
                _layersQueue.Enqueue(new DirtLayer(_prefabFactory));

            List<UniTask> tasks = new List<UniTask>();

            InitializeLayers(tasks);

            await UniTask.WhenAll(tasks);

            UnlockLayer();
        }

        private void InitializeLayers(List<UniTask> tasks)
        {
            foreach (DirtLayer layer in _layersQueue)
            {
                _depth++;
                layer.OnDig += HandleLayerDig;
                tasks.Add(layer.Initialize(LayerSizeX, LayerSizeY, _depth));
            }
        }

        private void HandleLayerDig() 
            => MoveNext();

        private void MoveNext()
        {
            _depth++;
            MoveDigLayerToEnd();
            UnlockLayer();
        }

        private void UnlockLayer()
        {
            DirtLayer nextLayer = _layersQueue.Peek();
            nextLayer.Unlock();
        }

        private void MoveDigLayerToEnd()
        {
            DirtLayer digLayer = _layersQueue.Dequeue();
            
            digLayer.Update(_depth).Forget();
            
            _layersQueue.Enqueue(digLayer);
        }
    }
}