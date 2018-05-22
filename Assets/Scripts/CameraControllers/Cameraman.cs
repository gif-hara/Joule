using UnityEngine;
using UnityEngine.Assertions;

namespace Joule.CameraControllers
{
    /// <summary>
    /// カメラを制御するクラス
    /// </summary>
    public sealed class Cameraman : MonoBehaviour
    {
        [SerializeField]
        private Transform root;
        public Transform Root
        {
            get { return root; }
        }

        [SerializeField]
        private Transform pivot;

        [SerializeField]
        private Transform dolly;

        [SerializeField]
        private Transform rig;

        [SerializeField]
        private Camera controlledCamera;
        public Camera ControlledCamera
        {
            get { return controlledCamera; }
        }
        
        
    }
}
