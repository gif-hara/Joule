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
                (this.cachedTransform.forward * Input.Vertical + this.cachedTransform.right * Input.Horizontal)
                .normalized;

            this.characterController.SimpleMove(velocity * speed * Time.deltaTime);
        }
    }
}
