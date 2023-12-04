using _Workspace.CodeBase.Infrastructure.Service.Assets.Data;
using _Workspace.CodeBase.Service.Factory;
using Cysharp.Threading.Tasks;

namespace _Workspace.CodeBase.UI.LoadingCurtain
{
    public class LoadingCurtainProxy : ILoadingCurtain
    {
        private readonly IPrefabFactoryAsync _factory;
        private ILoadingCurtain _curtain;

        public LoadingCurtainProxy(IPrefabFactoryAsync factory) 
            => _factory = factory;

        public async UniTask InitializeAsync() 
            => _curtain = await _factory.Create<LoadingCurtain>(InfrastructureAssetsAddress.CurtainAddress);

        public async UniTask Show() 
            => await _curtain.Show();

        public async UniTask Hide() 
            => await _curtain.Hide();

        public void Cancel() 
            => _curtain.Cancel();
    }
}