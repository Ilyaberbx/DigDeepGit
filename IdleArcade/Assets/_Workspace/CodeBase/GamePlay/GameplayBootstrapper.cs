using _Workspace.CodeBase.GamePlay.StateMachine;
using _Workspace.CodeBase.GamePlay.StateMachine.State;
using _Workspace.CodeBase.Infrastructure.Service.StateMachine.Factory;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Workspace.CodeBase.GamePlay
{
    public class GameplayBootstrapper : MonoBehaviour, IInitializable
    {
        private IStateFactory _stateFactory;
        private GameplayStateMachine _gameplayStateMachine;

        [Inject]
        public void Construct(GameplayStateMachine gameplayStateMachine,IStateFactory stateFactory)
        {
            _gameplayStateMachine = gameplayStateMachine;
            _stateFactory = stateFactory;
        }
        public void Initialize()
        {
            _gameplayStateMachine.RegisterState(_stateFactory.Create<GameplayBootstrapState>());

            _gameplayStateMachine.Enter<GameplayBootstrapState>().Forget();
        }
    }
}