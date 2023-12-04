using System;
using System.Collections.Generic;
using _Workspace.CodeBase.Infrastructure.Service.StateMachine.State;
using Cysharp.Threading.Tasks;

namespace _Workspace.CodeBase.Infrastructure.Service.StateMachine
{
    public abstract class StateMachine 
    {
        private readonly Dictionary<Type, IExitableState> registeredStates;
        private IExitableState currentState;

        public StateMachine() => 
            registeredStates = new Dictionary<Type, IExitableState>();

        public async UniTask Enter<TState>() where TState : class, IState
        {
            TState newState = await ChangeState<TState>();
            await newState.Enter();
        }

        public async UniTask Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            TState newState = await ChangeState<TState>();
            await newState.Enter(payload);
        }

        public void RegisterState<TState>(TState state) where TState : IExitableState =>
            registeredStates.Add(typeof(TState), state);

        private async UniTask<TState> ChangeState<TState>() where TState : class, IExitableState
        {
            if(currentState != null)
                await currentState.Exit();
      
            TState state = GetState<TState>();
            currentState = state;
      
            return state;
        }
    
        private TState GetState<TState>() where TState : class, IExitableState => 
            registeredStates[typeof(TState)] as TState;
    }
}