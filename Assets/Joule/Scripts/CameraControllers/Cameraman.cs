﻿using UnityEngine;
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

        private Vector3 targetPivot;

        private Vector3 pivotVelocity;

        private float targetDolly;

        private float dollyVelocity;

        void Awake()
        {
            this.targetPivot = this.pivot.localRotation.eulerAngles;
            this.targetDolly = this.dolly.localPosition.z;
        }

        void Update()
        {
            this.UpdatePivot();
            this.UpdateDolly();
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

        private void ClampPitch()
        {
            this.targetPivot.x = Mathf.Clamp(this.targetPivot.x, this.pitchMin, this.pitchMax);
        }

        private void UpdatePivot()
        {
            var r = this.pivot.localRotation.eulerAngles;
            var t = this.pitchSmoothTime * Time.deltaTime;
            this.pivot.localRotation = Quaternion.Euler(
                new Vector3(
                    Mathf.SmoothDampAngle(r.x, this.targetPivot.x, ref this.pivotVelocity.x, t),
                    Mathf.SmoothDampAngle(r.y, this.targetPivot.y, ref this.pivotVelocity.y, t),
                    Mathf.SmoothDampAngle(r.z, this.targetPivot.z, ref this.pivotVelocity.z, t)
                )
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
    }
}
