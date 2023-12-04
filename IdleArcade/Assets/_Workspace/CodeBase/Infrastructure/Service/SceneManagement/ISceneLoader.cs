using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace _Workspace.CodeBase.Infrastructure.Service.SceneManagement
{
    public interface ISceneLoader 
    {
        UniTask LoadAsync(string name, LoadSceneMode mode);
    }
}