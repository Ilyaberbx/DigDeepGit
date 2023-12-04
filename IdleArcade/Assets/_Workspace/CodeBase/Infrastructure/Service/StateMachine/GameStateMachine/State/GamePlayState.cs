using _Workspace.CodeBase.Infrastructure.Service.Assets;
using _Workspace.CodeBase.Infrastructure.Service.Assets.Data;
using _Workspace.CodeBase.Infrastructure.Service.SceneManagement;
using _Workspace.CodeBase.Infrastructure.Service.StateMachine.State;
using _Workspace.CodeBase.UI.LoadingCurtain;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace _Workspace.CodeBase.Infrastructure.Service.StateMachine.GameStateMachine.State
{
    public class GamePlayState : IState
    {
        private readonly ILoadingCurtain _curtain;
        private readonly IAssetsProvider _assets;
        private readonly ISceneLoader _sceneLoader;

        public GamePlayState(ILoadingCurtain curtain, IAssetsProvider assets, ISceneLoader sceneLoader)
        {
            _curtain = curtain;
            _assets = assets;
            _sceneLoader = sceneLoader;
        }

        public async UniTask Enter()
        {
            await _curtain.Show();
            
            await _assets.WarmUpAssetsByLabel(AssetsLabel.GamePlayState);
            await _sceneLoader.LoadAsync(InfrastructureAssetsAddress.GamePlayScene, LoadSceneMode.Single);
        }

        public async UniTask Exit()
            => await _assets.ReleaseAssetsByLabel(AssetsLabel.GamePlayState);
    }
}