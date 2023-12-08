using _Workspace.CodeBase.UI.Dirt;
using UnityEngine;

namespace _Workspace.CodeBase.GamePlay.Logic.DirtSystem
{
    public class DepthObstacle : MonoBehaviour
    {
        [SerializeField] private DepthCounter _counter;
        [SerializeField] private Renderer[] _renderers;
        
        public void Initialize(int depth, Color color)
        {
            _counter.UpdateDepthText(depth);
            
            foreach (Renderer renderer in _renderers) 
                renderer.material.color = color;
        }

        public void Dig()
            => Destroy(_counter.gameObject);
    }
}