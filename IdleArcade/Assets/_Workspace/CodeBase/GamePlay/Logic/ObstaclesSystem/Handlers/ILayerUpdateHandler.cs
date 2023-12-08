using _Workspace.CodeBase.Infrastructure.Service.EventBus.Handlers;
using UnityEngine;

namespace _Workspace.CodeBase.GamePlay.Logic.ObstaclesSystem.Handlers
{
    public interface ILayerUpdateHandler : IGlobalSubscriber
    {
        void HandLayerInitialize(int depth, Color layerColor);
    }
}