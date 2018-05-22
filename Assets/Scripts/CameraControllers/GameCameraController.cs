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

        private Vector3 chaseTrackVelocity;

        void Update()
        {
            this.ChaseTrack();
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
    }
}
