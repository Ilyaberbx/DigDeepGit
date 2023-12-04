using System.Collections.Generic;
using System.Threading.Tasks;
using _Workspace.CodeBase.GamePlay.Factory;
using _Workspace.CodeBase.GamePlay.Input;
using _Workspace.CodeBase.GamePlay.Logic.DirtSystem;
using _Workspace.CodeBase.Infrastructure.Service.EventBus.Handlers;
using _Workspace.CodeBase.Infrastructure.Service.StateMachine.State;
using _Workspace.CodeBase.Service.EventBus;
using _Workspace.CodeBase.UI.Factory;
using _Workspace.CodeBase.UI.LoadingCurtain;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Workspace.CodeBase.GamePlay.StateMachine.State
{
    public class GameplayBootstrapState : IState
    {
        private readonly IEventBusService _eventBus;
        private readonly ILoadingCurtain _curtain;
        private readonly IEnumerable<IGlobalSubscriber> _subscribers;
        private readonly IUIFactory _uiFactory;
        private readonly PlayerFactory _playerFactory;
        private readonly InputServiceProxy _inputProxy;
        private readonly DirtSystem _dirtSystem;
        private readonly GameplayStateMachine _gameplayStateMachine;

        public GameplayBootstrapState(IEventBusService eventBus
            , ILoadingCurtain curtain
            , IEnumerable<IGlobalSubscriber> subscribers
            , IUIFactory uiFactory
            , PlayerFactory playerFactory
            , InputServiceProxy inputProxy
            , DirtSystem dirtSystem
            , GameplayStateMachine gameplayStateMachine)
        {
            _eventBus = eventBus;
            _curtain = curtain;
            _subscribers = subscribers;
            _uiFactory = uiFactory;
            _playerFactory = playerFactory;
            _inputProxy = inputProxy;
            _dirtSystem = dirtSystem;
            _gameplayStateMachine = gameplayStateMachine;
        }

        public async UniTask Enter()
        {
            foreach (IGlobalSubscriber subscriber in _subscribers)
                _eventBus.Subscribe(subscriber);

            await _uiFactory.CreateUIRoot();

            await InitializeInputService();

            await InitializeDirtSystem();
            
            await _playerFactory.CreatePlayer(Vector3.zero);

            _curtain.Hide().Forget();
        }

        private async UniTask InitializeDirtSystem() 
            => await _dirtSystem.Initialize();

        private async UniTask InitializeInputService()
        {
            Joystick joystick = await _uiFactory.CreateJoyStick();
            _inputProxy.Initialize(joystick);
        }

        public UniTask Exit()
            => default;
    }
}