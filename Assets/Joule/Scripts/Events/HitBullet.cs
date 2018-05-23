using HK.Framework.EventSystems;
using Joule.BulletControllers;

namespace Joule.Events.BulletControllers
{
    /// <summary>
    /// 弾が当たった際のイベント
    /// </summary>
    public sealed class HitBullet : Message<HitBullet, BulletController>
    {
        /// <summary>
        /// 当たった弾
        /// </summary>
        public BulletController Bullet
        {
            get { return this.param1; }
        }
    }
}
