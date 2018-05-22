using HK.Framework.Extensions;
using Joule.Events.CharacterControllers;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace Joule.CharacterControllers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Character : MonoBehaviour
    {
        public CharacterStatus Status { get; private set; }

        private IMessageBroker broker;
        public IMessageBroker Broker
        {
            get
            {
                if (this.broker == null)
                {
                    this.broker = this.GetBroker();
                }
                return broker;
            }
        }

        void Awake()
        {
            this.Status = new CharacterStatus();
            
            HK.Framework.EventSystems.Broker.Global.Receive<Died>()
                .Where(x => x.Character == this)
                .Take(1)
                .SubscribeWithState(this, (_, _this) => { Destroy(_this.gameObject); })
                .AddTo(this);
        }

        public void Initialize(CharacterStatus status)
        {
            this.Status = status;
        }
    }
}
