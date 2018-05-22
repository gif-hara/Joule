using HK.Framework.EventSystems;
using Joule.Events.BulletControllers;
using Joule.Events.CharacterControllers;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace Joule.CharacterControllers
{
    /// <summary>
    /// キャラクターステータスを制御するクラス
    /// </summary>
    [RequireComponent(typeof(Character))]
    public sealed class CharacterStatusController : MonoBehaviour
    {
        private Character character;

        void Awake()
        {
            this.character = this.GetComponent<Character>();
            this.character.Broker.Receive<HitBullet>()
                .SubscribeWithState(this, (x, _this) => { _this.TakeDamage(x.Bullet.Damage); })
                .AddTo(this);
        }

        private void TakeDamage(int damage)
        {
            if (this.character.Status.IsDead)
            {
                return;
            }
            
            this.character.Status.HitPoint -= damage;
            if (this.character.Status.IsDead)
            {
                Broker.Global.Publish(Died.Get(this.character));
            }
        }
    }
}
