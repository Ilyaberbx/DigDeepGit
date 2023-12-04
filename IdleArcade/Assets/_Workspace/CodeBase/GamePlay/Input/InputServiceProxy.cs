using Zenject;

namespace _Workspace.CodeBase.GamePlay.Input
{
    public class InputServiceProxy : IInputService
    {
        private readonly IInstantiator _instantiator;
        private IInputService _inputService;

        public InputServiceProxy(IInstantiator instantiator)
            => _instantiator = instantiator;

        public void Initialize(Joystick joystick)
            => _inputService = _instantiator.Instantiate<JoyStickInputService>(new object[]
            {
                joystick
            });

        public float GetHorizontalInput()
            => _inputService.GetHorizontalInput();

        public float GetVerticalInput()
            => _inputService.GetVerticalInput();
    }
}