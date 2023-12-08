using System.Collections.Generic;
using System.Threading.Tasks;
using _Workspace.CodeBase.Extensions;
using _Workspace.CodeBase.GamePlay.Factory;
using _Workspace.CodeBase.GamePlay.Input;
using _Workspace.CodeBase.GamePlay.Logic.Camera;
using _Workspace.CodeBase.GamePlay.Logic.DirtSystem;
using _Workspace.CodeBase.GamePlay.Logic.LadderSystem;
using _Workspace.CodeBase.GamePlay.Logic.ObstaclesSystem;
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
        private readonly LadderWrapper _ladderWrapper;
        private readonly DepthObstacleWrapper _depthObstacleWrapper;
        private readonly CameraFollow _follow;

        private Vector3 PlayerStartPoint => Vector3.zero.WithZ(10);

        public GameplayBootstrapState(IEventBusService eventBus
            , ILoadingCurtain curtain
            , IEnumerable<IGlobalSubscriber> subscribers
            , IUIFactory uiFactory
            , PlayerFactory playerFactory
            , InputServiceProxy inputProxy
            , DirtSystem dirtSystem
            , LadderWrapper ladderWrapper
            , DepthObstacleWrapper depthObstacleWrapper
            , CameraFollow follow)
        {
            _eventBus = eventBus;
            _curtain = curtain;
            _subscribers = subscribers;
            _uiFactory = uiFactory;
            _playerFactory = playerFactory;
            _inputProxy = inputProxy;
            _dirtSystem = dirtSystem;
            _ladderWrapper = ladderWrapper;
            _depthObstacleWrapper = depthObstacleWrapper;
            _follow = follow;
        }

        public async UniTask Enter()
        {
            SubscribeHandlers();

            await InitializeUI();

            await InitializeServices();

            await InitializeWorld();

            _curtain.Hide()
                .Forget();
        }

        private void SubscribeHandlers()
        {
            foreach (IGlobalSubscriber subscriber in _subscribers)
                _eventBus.Subscribe(subscriber);
        }

        private async UniTask InitializeServices()
            => await InitializeInputService();

        private async UniTask InitializeUI()
            => await _uiFactory.CreateUIRoot();

        private async UniTask InitializeWorld()
        {
            await InitializeDepthObstacle();

            await InitializeLadder();

            await InitializeDirtSystem();

            Transform player = await _playerFactory.CreatePlayer(PlayerStartPoint);

            InitializeCamera(player);
        }

        private async UniTask InitializeDepthObstacle()
            => await _depthObstacleWrapper.Initialize();

        private async UniTask InitializeLadder()
            => await _ladderWrapper.Initialize();

        private void InitializeCamera(Transform player)
            => _follow.SetTarget(player);

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