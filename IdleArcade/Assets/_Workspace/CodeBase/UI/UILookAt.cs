using UnityEngine;

namespace _Workspace.CodeBase.UI
{
    public class UILookAt : MonoBehaviour
    {
        private void LateUpdate() 
            => LookAtCamera();

        private void LookAtCamera() 
            => transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }
}