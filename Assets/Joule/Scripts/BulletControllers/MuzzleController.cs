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

        private Transform cachedTransform;

        void Awake()
        {
            this.cachedTransform = this.transform;
        }

        public void Fire(Character owner)
        {
            var bullet = this.prefab.Rent(owner);
            bullet.CachedTransform.position = this.cachedTransform.position;
            bullet.CachedTransform.rotation = this.cachedTransform.rotation;
        }
    }
}
