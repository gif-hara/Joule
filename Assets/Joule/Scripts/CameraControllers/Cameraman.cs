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

        [SerializeField]
        private float pitchSmoothTime;

        [SerializeField]
        private float dollySmoothTime;

        [SerializeField]
        private float rigSmoothTime;

        private Vector3 targetPivot;

        private Vector3 targetRig;

        private Vector3 pivotVelocity;

        private Vector3 rigVelocity;

        private float targetDolly;

        private float dollyVelocity;

        void Awake()
        {
            this.targetPivot = this.pivot.localRotation.eulerAngles;
            this.targetDolly = this.dolly.localPosition.z;
            this.targetRig = this.rig.localRotation.eulerAngles;
        }

        void Update()
        {
            this.UpdatePivot();
            this.UpdateDolly();
            this.UpdateRig();
        }

        public void SetPivot(float pitch, float yaw)
        {
            this.targetPivot = new Vector3(pitch, yaw);
            this.ClampPitch();
        }

        public void AddPivot(float pitch, float yaw)
        {
            this.targetPivot.x += pitch;
            this.targetPivot.y += yaw;
            this.ClampPitch();
        }

        public void SetPivotYaw(float yaw)
        {
            this.targetPivot.y = yaw;
        }

        public void SetRig(float pitch, float yaw)
        {
            this.targetRig.x = pitch;
            this.targetRig.y = yaw;
        }

        private void ClampPitch()
        {
            this.targetPivot.x = Mathf.Clamp(this.targetPivot.x, this.pitchMin, this.pitchMax);
        }

        private void UpdatePivot()
        {
            var r = this.pivot.localRotation.eulerAngles;
            var t = this.pitchSmoothTime * Time.deltaTime;
            this.pivot.localRotation = Quaternion.Euler(
                Mathf.SmoothDampAngle(r.x, this.targetPivot.x, ref this.pivotVelocity.x, t),
                Mathf.SmoothDampAngle(r.y, this.targetPivot.y, ref this.pivotVelocity.y, t),
                Mathf.SmoothDampAngle(r.z, this.targetPivot.z, ref this.pivotVelocity.z, t)
            );
        }

        public void SetDolly(float dolly)
        {
            this.targetDolly = -dolly;
        }

        private void UpdateDolly()
        {
            var pos = this.dolly.localPosition;
            pos.z = Mathf.SmoothDamp(pos.z, this.targetDolly, ref this.dollyVelocity, this.dollySmoothTime * Time.deltaTime);
            this.dolly.localPosition = pos;
        }

        private void UpdateRig()
        {
            var r = this.rig.localRotation.eulerAngles;
            var t = this.rigSmoothTime * Time.deltaTime;
            this.rig.localRotation = Quaternion.Euler(
                Mathf.SmoothDampAngle(r.x, this.targetRig.x, ref this.rigVelocity.x, t),
                Mathf.SmoothDampAngle(r.y, this.targetRig.y, ref this.rigVelocity.y, t),
                Mathf.SmoothDampAngle(r.z, this.targetRig.z, ref this.rigVelocity.z, t)
                );
        }
    }
}
