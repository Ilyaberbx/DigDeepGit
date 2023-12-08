using _Workspace.CodeBase.Infrastructure.Service.EventBus.Handlers;

namespace _Workspace.CodeBase.GamePlay.Logic.ObstaclesSystem.Handlers
{
    public interface ILayerDigHandler : IGlobalSubscriber
    {
        void HandleLayerDig();
    }
}