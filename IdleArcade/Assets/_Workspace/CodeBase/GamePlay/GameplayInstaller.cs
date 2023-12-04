using _Workspace.CodeBase.GamePlay.Factory;
using _Workspace.CodeBase.GamePlay.Input;
using _Workspace.CodeBase.GamePlay.Logic.DirtSystem;
using _Workspace.CodeBase.GamePlay.StateMachine;
using _Workspace.CodeBase.Service.Factory;
using _Workspace.CodeBase.UI.Factory;
using UnityEngine;
using Zenject;

namespace _Workspace.CodeBase.GamePlay
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private GameplayBootstrapper _bootstrapper;

        public override void InstallBindings()
        {
            BindInputService();
            BindPrefabFactory();
            BindStateMachine();
            BindUIFactory();
            BindPlayerFactory();
            BindDirtSystem();
            BindBootstrapper();
        }

        private void BindDirtSystem()
            => Container.Bind<DirtSystem>()
                .AsSingle();

        private void BindPlayerFactory()
            => Container.Bind<PlayerFactory>()
                .AsSingle();

        private void BindUIFactory()
            => Container.BindInterfacesTo<UIFactory>()
                .AsSingle();

        private void BindInputService()
            => Container.BindInterfacesAndSelfTo<InputServiceProxy>()
                .AsSingle();


        private void BindStateMachine()
            => Container.Bind<GameplayStateMachine>()
                .AsSingle();

        private void BindPrefabFactory()
            => Container.BindInterfacesTo<PrefabFactoryAsync>()
                .AsSingle();

        private void BindBootstrapper()
            => Container.BindInterfacesTo<GameplayBootstrapper>()
                .FromInstance(_bootstrapper)
                .AsSingle();
    }
}