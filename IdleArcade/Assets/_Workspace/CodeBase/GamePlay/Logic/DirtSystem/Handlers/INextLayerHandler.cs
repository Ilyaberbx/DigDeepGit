using _Workspace.CodeBase.Infrastructure.Service.EventBus.Handlers;

namespace _Workspace.CodeBase.GamePlay.Logic.DirtSystem.Handlers
{
    public interface INextLayerHandler : IGlobalSubscriber
    {
        void HandleNextLayer(int depth);
    }
}