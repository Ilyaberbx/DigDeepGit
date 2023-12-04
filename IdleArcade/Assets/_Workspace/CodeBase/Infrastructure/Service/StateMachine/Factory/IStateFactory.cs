using _Workspace.CodeBase.Infrastructure.Service.StateMachine.State;

namespace _Workspace.CodeBase.Infrastructure.Service.StateMachine.Factory
{
    public interface IStateFactory
    {
        TState Create<TState>() where TState : IExitableState;
    }
}