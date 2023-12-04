using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace _Workspace.CodeBase.Infrastructure.Service.StateMachine.State
{
    public interface IExitableState
    {
        UniTask Exit();
    }
}