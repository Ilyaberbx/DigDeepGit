using UnityEngine;
using Zenject;

namespace _Workspace.CodeBase.GamePlay.Logic.Camera.Installer
{
    public class CameraInstaller : MonoInstaller
    {
        [SerializeField] private CameraFollow _cameraFollow;
        
        public override void InstallBindings() =>
            Container.Bind<CameraFollow>()
                .FromInstance(_cameraFollow)
                .AsSingle();
    }
}