using _Workspace.CodeBase.Infrastructure.Service.EventBus.Handlers;

namespace _Workspace.CodeBase.UI.Player.Handler
{
    public interface IGemsCountUpdateHandler : IGlobalSubscriber
    {
        void HandleGemsCountUpdate(int count);
    }
}