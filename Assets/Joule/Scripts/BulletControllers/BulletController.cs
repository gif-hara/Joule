using System;
using HK.Framework;
using Joule.CharacterControllers;
using Joule.Events.BulletControllers;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace Joule.BulletControllers
{
    /// <summary>
    /// 弾を制御するクラス
    /// </summary>
    public sealed class BulletController : MonoBehaviour
    {
        public static readonly ObjectPoolBundle<BulletController> Bundle = new ObjectPoolBundle<BulletController>();
        
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

        private ObjectPool<BulletController> pool;

        private int ownerLayer;

        public Transform CachedTransform { get; private set; }

        void Awake()
        {
            this.CachedTransform = this.transform;
        }

        void Update()
        {
            this.CachedTransform.position += this.CachedTransform.forward * this.moveSpeed * Time.deltaTime;
        }

        public BulletController Rent(Character owner)
        {
            var pool = Bundle.Get(this);
            var instance = pool.Rent();
            instance.pool = pool;
            instance.ownerLayer = owner.gameObject.layer;
            instance.penetration = this.penetration;
            Observable.Timer(TimeSpan.FromSeconds(instance.duration))
                .TakeUntilDisable(instance)
                .SubscribeWithState(instance, (_, _this) => _this.pool.Return(_this))
                .AddTo(instance);

            return instance;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            var character = other.GetComponentInParent<Character>();

            if (character == null)
            {
                this.OnCollideOther();
                return;
            }

            if (this.ownerLayer != character.gameObject.layer)
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
                    this.pool.Return(this);
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
