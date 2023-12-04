using _Workspace.CodeBase.GameLoading.StateMachine;
using UnityEngine;
using Zenject;

namespace _Workspace.CodeBase.GameLoading
{
    public class GameLoadingInstaller : MonoInstaller
    {
        [SerializeField] private GameLoadingBootstrapper _bootstrapper;
        public override void InstallBindings()
        {
            BindStateMachine();
            BindBootstrapper();
        }

        private void BindBootstrapper()
            => Container.BindInterfacesTo<GameLoadingBootstrapper>()
                .FromInstance(_bootstrapper)
                .AsSingle();

        private void BindStateMachine() 
            => Container.Bind<LoadingStateMachine>().AsSingle();
    }
}