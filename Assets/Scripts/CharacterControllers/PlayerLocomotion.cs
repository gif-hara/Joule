using Joule.GameSystems;
using UnityEngine;
using UnityEngine.Assertions;

namespace Joule.CharacterControllers
{
    /// <summary>
    /// プレイヤーの移動を制御する
    /// </summary>
    public sealed class PlayerLocomotion : MonoBehaviour
    {
        [SerializeField]
        private CharacterController characterController;

        [SerializeField]
        private float speed;

        private Transform cachedTransform;

        void Awake()
        {
            this.cachedTransform = this.transform;
        }
        
        void Update()
        {
            var velocity =
                (ThirdPersonUtility.Forward * Input.GetAxis(ButtonNames.MoveVertical) + ThirdPersonUtility.Right * Input.GetAxis(ButtonNames.MoveHorizontal))
                .normalized;

            this.characterController.SimpleMove(velocity * speed * Time.deltaTime);

            var lockon = Input.GetButton(ButtonNames.Lockon);
            if (!lockon && velocity.sqrMagnitude > 0.0f)
            {
                this.cachedTransform.forward = velocity;
            }
            else
            {
                var yaw = Input.GetAxis(ButtonNames.CameraHorizontal) * Options.Instance.Data.CameraSpeed * Time.deltaTime;
                this.cachedTransform.rotation *= Quaternion.Euler(0.0f, yaw, 0.0f);
            }
        }
    }
}
