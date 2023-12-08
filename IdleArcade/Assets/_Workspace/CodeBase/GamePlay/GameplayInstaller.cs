using _Workspace.CodeBase.GamePlay.Factory;
using _Workspace.CodeBase.GamePlay.Input;
using _Workspace.CodeBase.GamePlay.Logic.DirtSystem;
using _Workspace.CodeBase.GamePlay.Logic.DirtSystem.StaticData;
using _Workspace.CodeBase.GamePlay.Logic.GemSystem;
using _Workspace.CodeBase.GamePlay.Logic.LadderSystem;
using _Workspace.CodeBase.GamePlay.Logic.ObstaclesSystem;
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
        [SerializeField] private DirtConfig _dirtConfig;


        public override void InstallBindings()
        {
            BindInputService();
            BindPrefabFactory();
            BindStateMachine();
            BindUIFactory();
            BindPlayerFactory();
            BindDirtSystem();
            BindGemsProvider();
            BindLadderWrapper();
            BindPitObstacleWrapper();
            BindBootstrapper();
        }

        private void BindPitObstacleWrapper() 
            => Container.BindInterfacesAndSelfTo<DepthObstacleWrapper>()
                .AsSingle();

        private void BindLadderWrapper() 
            => Container.BindInterfacesAndSelfTo<LadderWrapper>()
                .AsSingle();

        private void BindGemsProvider()
            => Container.BindInterfacesTo<GemsProvider>()
                .AsSingle();

        private void BindDirtSystem()
            => Container.BindInterfacesAndSelfTo<DirtSystem>()
                .AsSingle()
                .WithArguments(_dirtConfig);

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