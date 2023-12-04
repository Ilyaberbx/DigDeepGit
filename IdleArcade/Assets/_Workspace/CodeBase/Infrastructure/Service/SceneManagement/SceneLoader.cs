using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace _Workspace.CodeBase.Infrastructure.Service.SceneManagement
{
    public class SceneLoader : ISceneLoader
    {
        public async UniTask LoadAsync(string name, LoadSceneMode mode)
        {
            AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(name, mode, false);
            
            await handle.ToUniTask();
            
            await handle.GetAwaiter()
                .GetResult()
                .ActivateAsync()
                .ToUniTask();
        }
    }
}