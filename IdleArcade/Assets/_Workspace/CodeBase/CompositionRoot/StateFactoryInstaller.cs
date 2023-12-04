using _Workspace.CodeBase.Infrastructure.Service.StateMachine.Factory;
using Zenject;

namespace _Workspace.CodeBase.CompositionRoot
{
    public class StateFactoryInstaller : MonoInstaller
    {
        public override void InstallBindings() 
            => BindStateFactory();

        private void BindStateFactory() 
            => Container.BindInterfacesTo<StateFactory>()
                .AsSingle();
    }
}