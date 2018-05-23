using Joule.CharacterControllers;
using Joule.Events.BulletControllers;
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
        private int damage;
        public int Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        /// <summary>
        /// 貫通できる回数
        /// </summary>
        /// <remarks>
        /// <c>-1</c>にした場合は常に貫通する
        /// </remarks>
        [SerializeField]
        private int penetration;

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
            var character = other.GetComponentInParent<Character>();

            if (character == null)
            {
                this.OnCollideOther();
                return;
            }

            if (this.owner.gameObject.layer != character.gameObject.layer)
            {
                this.OnCollideOpponent(character);
            }
        }

        /// <summary>
        /// 敵対するオブジェクトと衝突した際の処理
        /// </summary>
        private void OnCollideOpponent(Character character)
        {
            character.Broker.Publish(HitBullet.Get(this));
            
            if (this.penetration != -1)
            {
                this.penetration--;
                if (this.penetration <= 0)
                {
                    Destroy(this.gameObject);
                }
            }
        }

        /// <summary>
        /// 何かしらのオブジェクトと衝突した際の処理
        /// </summary>
        private void OnCollideOther()
        {
            
        }
    }
}
