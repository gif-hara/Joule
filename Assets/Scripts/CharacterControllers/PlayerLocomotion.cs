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
                (ThirdPersonUtility.Forward * Input.GetAxis(ButtonNames.Vertical) + ThirdPersonUtility.Right * Input.GetAxis(ButtonNames.Horizontal))
                .normalized;

            this.characterController.SimpleMove(velocity * speed * Time.deltaTime);
            if (velocity.sqrMagnitude > 0.0f)
            {
                this.cachedTransform.forward = velocity;
            }
        }
    }
}
