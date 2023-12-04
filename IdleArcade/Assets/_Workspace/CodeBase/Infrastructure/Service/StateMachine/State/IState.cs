using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace _Workspace.CodeBase.Infrastructure.Service.StateMachine.State
{
    public interface IState : IExitableState
    {
        UniTask Enter();
    }
}