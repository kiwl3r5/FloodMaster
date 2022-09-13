using UnityEngine;

namespace Script
{
    public class LookatCam : MonoBehaviour
    {
        private Camera _mCamera;

        private void Awake()
        {
            _mCamera = Camera.main;
        }

        private void Update()
        {
            var cameraTransform = _mCamera.transform;
            Vector3 targetVector = this.transform.position - cameraTransform.position;
            transform.rotation = Quaternion.LookRotation(targetVector,cameraTransform.rotation*Vector3.up);
        }
    }
}
