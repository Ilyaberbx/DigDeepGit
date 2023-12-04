using System.Threading.Tasks;
using _Workspace.CodeBase.Service.Factory;
using _Workspace.CodeBase.UI.Factory.Assets;

namespace _Workspace.CodeBase.UI.Factory
{
    public class UIFactory : IUIFactory
    {
        private readonly IPrefabFactoryAsync _prefabFactory;
        private UIRoot _uiRoot;

        public UIFactory(IPrefabFactoryAsync prefabFactory) 
            => _prefabFactory = prefabFactory;

        public async Task<UIRoot> CreateUIRoot() 
            => _uiRoot = await _prefabFactory.Create<UIRoot>(UIAssetsAddress.UIRoot);
        
        public async Task<Joystick> CreateJoyStick() 
            => await _prefabFactory.Create<Joystick>(UIAssetsAddress.Joystick, _uiRoot.transform);
    }
}