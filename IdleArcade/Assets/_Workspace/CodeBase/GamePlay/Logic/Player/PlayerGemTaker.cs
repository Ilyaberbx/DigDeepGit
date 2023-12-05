using _Workspace.CodeBase.GamePlay.Logic.GemSystem;
using UnityEngine;

namespace _Workspace.CodeBase.GamePlay.Logic.Player
{
    public class PlayerGemTaker : MonoBehaviour
    {
        [SerializeField] private GemsBag _bag;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Gem gem)) 
                _bag.Store(gem);
        }
    }
}