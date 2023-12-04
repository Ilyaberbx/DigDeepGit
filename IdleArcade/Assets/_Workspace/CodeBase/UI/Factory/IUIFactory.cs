using System.Threading.Tasks;

namespace _Workspace.CodeBase.UI.Factory
{
    public interface IUIFactory
    {
        Task<UIRoot> CreateUIRoot();
        Task<Joystick> CreateJoyStick();
    }
}