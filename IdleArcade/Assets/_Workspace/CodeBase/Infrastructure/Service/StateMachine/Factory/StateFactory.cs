using _Workspace.CodeBase.Infrastructure.Service.StateMachine.State;
using Zenject;

namespace _Workspace.CodeBase.Infrastructure.Service.StateMachine.Factory
{
    public class StateFactory : IStateFactory
    {
        private readonly IInstantiator _instantiator;

        public StateFactory(IInstantiator instantiator) => 
            _instantiator = instantiator;

        public TState Create<TState>() where TState : IExitableState => 
            _instantiator.Instantiate<TState>();
    }
}