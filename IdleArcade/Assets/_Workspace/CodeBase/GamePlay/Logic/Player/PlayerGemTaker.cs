using _Workspace.CodeBase.GamePlay.Logic.GemSystem;
using UnityEngine;

namespace _Workspace.CodeBase.GamePlay.Logic.Player
{
    public class PlayerGemTaker : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Gem gem))
            {
                Debug.Log("Gem");
                gem.gameObject.SetActive(false);
            }
        }
    }
}