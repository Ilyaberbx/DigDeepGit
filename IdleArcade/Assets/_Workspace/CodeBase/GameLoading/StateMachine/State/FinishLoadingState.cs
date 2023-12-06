using _Workspace.CodeBase.Infrastructure.Service.StateMachine.GameStateMachine;
using _Workspace.CodeBase.Infrastructure.Service.StateMachine.GameStateMachine.State;
using _Workspace.CodeBase.Infrastructure.Service.StateMachine.State;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Workspace.CodeBase.GameLoading.StateMachine.State
{
    public class FinishLoadingState : IState
    {
        private readonly GameStateMachine _gameStateMachine;

        public FinishLoadingState(GameStateMachine gameStateMachine) 
            => _gameStateMachine = gameStateMachine;

        public UniTask Enter()
        {
            Debug.Log("Finish loading");
            _gameStateMachine.Enter<GamePlayState>().Forget();
            return default;
        }

        public UniTask Exit() 
            => default;
    }
}