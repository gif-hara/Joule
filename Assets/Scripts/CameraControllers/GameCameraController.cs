using HK.Framework.EventSystems;
using Joule.Events.CharacterControllers;
using Joule.GameSystems;
using UniRx;
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

        void Awake()
        {
            Broker.Global.Receive<PlayerSpawned>()
                .Take(1)
                .SubscribeWithState(this, (x, _this) =>
                {
                    _this.track = x.Player.CachedTransform;
                    _this.cameraman.Root.position = _this.track.position;
                })
                .AddTo(this);
        }

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

            this.cameraman.Root.position = Vector3.SmoothDamp(
                this.cameraman.Root.localPosition,
                this.track.position,
                ref this.chaseTrackVelocity,
                this.chaseTrackSmoothTime * Time.deltaTime
            );
        }

        private void UpdatePivot()
        {
            var startLockon = Input.GetButtonDown(ButtonNames.Lockon);
            if (startLockon)
            {
                this.StartPivotOnThirdPerson();
            }
            else
            {
                this.UpdatePivotFree();
            }
        }

        private void UpdatePivotFree()
        {
            var yaw = Input.GetAxis(ButtonNames.CameraHorizontal);
            var pitch = Input.GetAxis(ButtonNames.CameraVertical);
            var pivot = new Vector2(yaw, pitch).normalized * this.pivotSpeed * Time.deltaTime;
            this.cameraman.AddPivot(pivot.x, pivot.y);
        }

        private void StartPivotOnThirdPerson()
        {
            if (this.track == null)
            {
                return;
            }

            var yaw = this.track.rotation.eulerAngles.y;
            this.cameraman.SetPivotYaw(yaw);
        }
    }
}
