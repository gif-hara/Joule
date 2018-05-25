using HK.Framework.EventSystems;
using Joule.Events.CharacterControllers;
using Joule.GameSystems;
using UniRx;
using UniRx.Triggers;
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
        private float defaultChaseTrackSmoothTime;
        
        [SerializeField]
        private float lockonChaseTrackSmoothTime;

        [SerializeField]
        private float defaultDolly;

        [SerializeField]
        private float lockonDolly;

        [SerializeField]
        private LayerMask backCheckLayer;

        [SerializeField]
        private float lockonTrackOffsetX;

        [SerializeField]
        private float lockonTrackOffsetXThreshold;

        [SerializeField]
        private float defaultRigPitch;

        [SerializeField]
        private float lockonRigPitch;

        private Vector3 chaseTrackVelocity;

        private Vector3 pivotVelocity;

        private Vector3 rigVelocity;

        private float currentLockonTrackOffsetX;

        void Awake()
        {
            this.currentLockonTrackOffsetX = this.lockonTrackOffsetX;
            Broker.Global.Receive<PlayerSpawned>()
                .Take(1)
                .SubscribeWithState(this, (x, _this) =>
                {
                    _this.CreateTrack(x.Player.CachedTransform);
                    _this.cameraman.Root.position = _this.track.position;
                })
                .AddTo(this);
        }

        void LateUpdate()
        {
            this.ChaseTrack();
            this.UpdatePivot();
            this.UpdateDolly();
            this.UpdateLockonTrackOffsetX();
            this.UpdateRig();
        }

        private void ChaseTrack()
        {
            if (this.track == null)
            {
                return;
            }

            var lockon = Input.GetButton(ButtonNames.Lockon);
            var smoothTime = lockon ? this.lockonChaseTrackSmoothTime : this.defaultChaseTrackSmoothTime;
            this.cameraman.Root.position = Vector3.SmoothDamp(
                this.cameraman.Root.localPosition,
                this.track.position,
                ref this.chaseTrackVelocity,
                smoothTime * Time.deltaTime
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
            var t = Options.Instance.Data.CameraSpeed * Time.deltaTime;
            this.cameraman.AddPivot(pitch * t, yaw * t);
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

        private void UpdateDolly()
        {
            var lockon = Input.GetButton(ButtonNames.Lockon);
            this.cameraman.SetDolly(lockon ? this.lockonDolly : this.defaultDolly);
        }

        private void UpdateLockonTrackOffsetX()
        {
            var horizontal = Input.GetAxis(ButtonNames.CameraHorizontal);
            if (Mathf.Approximately(horizontal, 0.0f))
            {
                return;
            }

            if (this.lockonTrackOffsetXThreshold > Mathf.Abs(horizontal))
            {
                return;
            }
            
            horizontal = Mathf.Sign(horizontal);

            this.currentLockonTrackOffsetX = this.lockonTrackOffsetX * horizontal;
        }

        private void CreateTrack(Transform original)
        {
            this.track = new GameObject("GameCameraTrack").transform;
            this.LateUpdateAsObservable()
                .Where(_ => this.isActiveAndEnabled)
                .SubscribeWithState2(this, original, (_, _this, o) =>
                {
                    var pos = o.position;
                    var lockon = Input.GetButton(ButtonNames.Lockon);
                    if (lockon)
                    {
                        pos += o.right * _this.currentLockonTrackOffsetX;
                    }
                    _this.track.position = pos;
                    _this.track.rotation = o.rotation;
                })
                .AddTo(this);
        }

        private void UpdateRig()
        {
            var lockon = Input.GetButton(ButtonNames.Lockon);
            this.cameraman.SetRig(lockon ? this.lockonRigPitch : this.defaultRigPitch, 0.0f);
        }
    }
}
