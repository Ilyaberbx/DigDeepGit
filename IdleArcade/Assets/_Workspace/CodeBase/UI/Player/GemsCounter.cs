using _Workspace.CodeBase.UI.Player.Handler;
using TMPro;
using UnityEngine;

namespace _Workspace.CodeBase.UI.Player
{
    public class GemsCounter : MonoBehaviour, IGemsCountUpdateHandler
    {
        [SerializeField] private TextMeshProUGUI _text;
        
        public void HandleGemsCountUpdate(int count) 
            => _text.text = count.ToString();
    }
}