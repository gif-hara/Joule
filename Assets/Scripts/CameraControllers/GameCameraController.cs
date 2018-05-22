using Joule.GameSystems;
using UnityEngine;
using UnityEngine.Assertions;

namespace Joule.CameraControllers
{
    /// <summary>
    /// ゲーム中のカメラを制御するクラス
    /// </summary>
    public sealed class GameCameraController : MonoBehaviour
    {
        [SerializeField]
        private Cameraman cameraman;

        [SerializeField]
        private Transform track;
        public Transform Track
        {
            get { return track; }
            set { track = value; }
        }

        [SerializeField]
        private float chaseTrackSmoothTime;

        [SerializeField]
        private float pivotSpeed;

        private Vector3 chaseTrackVelocity;

        void Update()
        {
            this.ChaseTrack();
            this.UpdatePivot();
        }

        private void ChaseTrack()
        {
            if (this.track == null)
            {
                return;
            }

            this.cameraman.Root.localPosition = Vector3.SmoothDamp(
                this.cameraman.Root.localPosition,
                this.track.position,
                ref this.chaseTrackVelocity,
                this.chaseTrackSmoothTime * Time.deltaTime
            );
        }

        private void UpdatePivot()
        {
            var pivot = new Vector2(Input.GetAxis(ButtonNames.MouseX), Input.GetAxis(ButtonNames.MouseY)).normalized * this.pivotSpeed * Time.deltaTime;
            this.cameraman.AddPivot(pivot.x, pivot.y);
        }
    }
}
