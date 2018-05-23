using Joule.BulletControllers;
using Joule.CharacterControllers;
using UnityEngine;
using UnityEngine.Assertions;

namespace Joule
{
    /// <summary>
    /// 弾を発射するクラス
    /// </summary>
    public sealed class MuzzleController : MonoBehaviour
    {
        [SerializeField]
        private BulletController prefab;
        public BulletController Prefab
        {
            get { return prefab; }
            set { prefab = value; }
        }

        [SerializeField]
        private float coolTime;

        private Transform cachedTransform;

        private float currentCoolTime;

        void Awake()
        {
            this.cachedTransform = this.transform;
            this.currentCoolTime = this.coolTime;
        }

        void Update()
        {
            this.currentCoolTime += Time.deltaTime;
        }

        public void Fire(Character owner)
        {
            if (!this.CanFire)
            {
                return;
            }

            this.currentCoolTime = 0.0f;
            var bullet = this.prefab.Rent(owner);
            bullet.CachedTransform.position = this.cachedTransform.position;
            bullet.CachedTransform.rotation = this.cachedTransform.rotation;
        }

        public bool CanFire
        {
            get { return this.currentCoolTime >= this.coolTime; }
        }
    }
}
