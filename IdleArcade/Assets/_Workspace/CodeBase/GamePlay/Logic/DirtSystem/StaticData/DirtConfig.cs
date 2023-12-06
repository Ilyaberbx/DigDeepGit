using UnityEngine;

namespace _Workspace.CodeBase.GamePlay.Logic.DirtSystem.StaticData
{
    [CreateAssetMenu(fileName = "DirtConfig", menuName = "StaticData/DirtConfig", order = 0)]
    public class DirtConfig : ScriptableObject
    {
        [field: SerializeField] public Color OddColor { get; private set; }
        [field: SerializeField] public Color EvenColor { get; private set; }
    }
}