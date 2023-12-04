using _Workspace.CodeBase.GameLoading.StateMachine;
using _Workspace.CodeBase.GameLoading.StateMachine.State;
using _Workspace.CodeBase.Infrastructure.Service.StateMachine.Factory;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Workspace.CodeBase.GameLoading
{
    public class GameLoadingBootstrapper : MonoBehaviour, IInitializable
    {
        private LoadingStateMachine _loadingStateMachine;
        private IStateFactory _stateFactory;

        [Inject]
        public void Construct(LoadingStateMachine loadingStateMachine, IStateFactory stateFactory)
        {
            _loadingStateMachine = loadingStateMachine;
            _stateFactory = stateFactory;
        }
        public void Initialize()
        {
            _loadingStateMachine.RegisterState(_stateFactory.Create<FinishLoadingState>());

            _loadingStateMachine.Enter<FinishLoadingState>().Forget();
        }
    }
}