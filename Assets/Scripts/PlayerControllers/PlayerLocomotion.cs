using Joule.GameSystems;
using UnityEngine;
using UnityEngine.Assertions;
using Input = Joule.GameSystems.Input;

namespace Joule.PlayerControllers
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
                (ThirdPersonUtility.Forward * Input.Vertical + ThirdPersonUtility.Right * Input.Horizontal)
                .normalized;

            this.characterController.SimpleMove(velocity * speed * Time.deltaTime);
            if (velocity.sqrMagnitude > 0.0f)
            {
                this.cachedTransform.forward = velocity;
            }
        }
    }
}
