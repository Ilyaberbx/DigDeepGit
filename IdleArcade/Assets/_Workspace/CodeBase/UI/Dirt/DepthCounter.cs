using TMPro;
using UnityEngine;

namespace _Workspace.CodeBase.UI.Dirt
{
    public class DepthCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        public void UpdateDepthText(int depth) 
            => _text.text = depth.ToString();
    }
}