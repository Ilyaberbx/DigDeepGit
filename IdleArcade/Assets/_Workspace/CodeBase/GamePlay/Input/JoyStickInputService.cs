using UnityEngine;

namespace _Workspace.CodeBase.GamePlay.Input
{
    public class JoyStickInputService : IInputService
    {
        private readonly Joystick _joystick;

        public JoyStickInputService(Joystick joystick)
            => _joystick = joystick;

        public float GetHorizontalInput()
            => _joystick.Horizontal;

        public float GetVerticalInput()
            => _joystick.Vertical;
    }
}