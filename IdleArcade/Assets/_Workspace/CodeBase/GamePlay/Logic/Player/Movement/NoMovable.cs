using UnityEngine;

namespace _Workspace.CodeBase.GamePlay.Logic.Player.Movement
{
    public class NoMovable : IMovable
    {
        public Vector3 MoveWithDirection() 
            => Vector3.zero;
    }
}