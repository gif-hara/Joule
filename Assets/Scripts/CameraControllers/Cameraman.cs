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

        [SerializeField]
        private float pitchMax;

        [SerializeField]
        private float pitchMin;

        public void SetPivot(float yaw, float pitch)
        {
            this.pivot.localRotation = Quaternion.Euler(
                Mathf.Clamp(pitch, this.pitchMin, this.pitchMax),
                yaw,
                0.0f
                );
        }

        public void AddPivot(float yaw, float pitch)
        {
            var euler = this.pivot.localRotation.eulerAngles;
            euler.y += yaw;
            euler.x = Mathf.Clamp(euler.x + pitch, this.pitchMin, this.pitchMax);
            this.pivot.localRotation = Quaternion.Euler(euler);
        }

        public void SetPivotYaw(float yaw)
        {
            var r = this.pivot.localRotation.eulerAngles;
            this.pivot.localRotation = Quaternion.Euler(
                r.x,
                yaw,
                r.z
            );
        }
    }
}
