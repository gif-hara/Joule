using Joule.CharacterControllers;
using UnityEngine;
using UnityEngine.Assertions;

namespace Joule.BulletControllers
{
    /// <summary>
    /// 弾を制御するクラス
    /// </summary>
    public sealed class BulletController : MonoBehaviour
    {
        [SerializeField]
        private float duration;

        [SerializeField]
        private float moveSpeed;

        [SerializeField]
        private float damage;

        private Character owner;

        private Transform cachedTransform;

        void Awake()
        {
            this.cachedTransform = this.transform;
        }

        void Update()
        {
            this.cachedTransform.position += this.cachedTransform.forward * this.moveSpeed * Time.deltaTime;
        }

        public void Initialize(Character owner)
        {
            this.owner = owner;
            Destroy(this.gameObject, this.duration);
        }
        
        private void OnTriggerEnter(Collider other)
        {
        }
    }
}
