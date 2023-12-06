using _Workspace.CodeBase.GamePlay.Assets;
using _Workspace.CodeBase.Infrastructure.Service.EventBus.Handlers;
using _Workspace.CodeBase.Service.EventBus;
using _Workspace.CodeBase.Service.Factory;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Workspace.CodeBase.GamePlay.Factory
{
    public class PlayerFactory
    {
        private readonly IPrefabFactoryAsync _prefabFactory;
        private readonly IEventBusService _eventBus;

        public PlayerFactory(IPrefabFactoryAsync prefabFactory, IEventBusService eventBus)
        {
            _prefabFactory = prefabFactory;
            _eventBus = eventBus;
        }

        public async UniTask<Transform> CreatePlayer(Vector3 at)
        {
           Transform player = await _prefabFactory.Create<Transform>(GamePlayAssetsAddress.Player, at, null);

           IGlobalSubscriber[] globalSubscribers = player.GetComponentsInChildren<IGlobalSubscriber>();
           
           if (globalSubscribers != null)
               foreach (var subscriber in globalSubscribers)
                   _eventBus.Subscribe(subscriber);

           return player;
        }
    }
}