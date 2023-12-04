using UnityEngine;

namespace _Workspace.CodeBase.UI
{
    public class BaseWindow : MonoBehaviour
    {
        public void Close()
            => Destroy(gameObject);
    }
}