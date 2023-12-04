using _Workspace.CodeBase.Infrastructure.Service.Assets;
using _Workspace.CodeBase.Infrastructure.Service.SceneManagement;
using _Workspace.CodeBase.Infrastructure.Service.StateMachine.GameStateMachine;
using _Workspace.CodeBase.Service.EventBus;
using _Workspace.CodeBase.Service.Factory;
using _Workspace.CodeBase.UI.LoadingCurtain;
using UnityEngine;
using Zenject;

namespace _Workspace.CodeBase.CompositionRoot
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindAssetsProviderService();
            BindEventBusService();
            BindLoadingCurtain();
            BindSceneLoaderService();
            BindGameStateMachine();
        }
        
        
        private void BindSceneLoaderService()
            => Container.BindInterfacesTo<SceneLoader>().AsSingle();

        private void BindEventBusService()
            => Container.BindInterfacesTo<EventBusService>().AsSingle();

        private void BindAssetsProviderService()
            => Container.BindInterfacesTo<AssetsProvider>().AsSingle();

        private void BindLoadingCurtain()
        {
            Container.BindInterfacesTo<PrefabFactoryAsync>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<LoadingCurtainProxy>()
                .AsSingle();
        }

        private void BindGameStateMachine()
        {
            Container.Bind<GameStateMachine>().AsSingle();
            Debug.Log("Game state machine installed");
        }
    }
}