using _Workspace.CodeBase.Infrastructure.Service.StateMachine.Factory;
using _Workspace.CodeBase.Infrastructure.Service.StateMachine.GameStateMachine;
using _Workspace.CodeBase.Infrastructure.Service.StateMachine.GameStateMachine.State;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Workspace.CodeBase.CompositionRoot
{
    public class GameBootstrapper : MonoBehaviour
    {
        private GameStateMachine _gameStateMachine;
        private IStateFactory _factory;

        [Inject]
        public void Construct(GameStateMachine gameStateMachine, IStateFactory factory)
        {
            _gameStateMachine = gameStateMachine;
            _factory = factory;
        }

        private void Start()
        {
            _gameStateMachine.RegisterState(_factory.Create<BootStrapState>());
            _gameStateMachine.RegisterState(_factory.Create<GameLoadingState>());
            _gameStateMachine.RegisterState(_factory.Create<GamePlayState>());
            
            _gameStateMachine.Enter<BootStrapState>().Forget();
            
            DontDestroyOnLoad(this);
        }
    }
}