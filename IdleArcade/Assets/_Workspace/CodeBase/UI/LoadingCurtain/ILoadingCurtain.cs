using Cysharp.Threading.Tasks;

namespace _Workspace.CodeBase.UI.LoadingCurtain
{
    public interface ILoadingCurtain
    {
        UniTask Show();
        UniTask Hide();
        void Cancel();
    }
}