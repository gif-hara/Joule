using HK.Framework;
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
        private CharacterStatus status;
        public CharacterStatus Status
        {
            get
            {
                if (this.status == null)
                {
                    this.status = new CharacterStatus();
                }

                return this.status;
            }
            
        }

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
        
        public Transform CachedTransform { get; private set; }

        void Awake()
        {
            this.CachedTransform = this.transform;
        }

        public void Initialize(CharacterBlueprint blueprint)
        {
            this.status = new CharacterStatus(blueprint.Status);
            var m = Instantiate(blueprint.Model, this.CachedTransform);
            m.transform.localPosition = Vector3.zero;
            m.transform.localRotation = Quaternion.identity;
            if (blueprint.AI != null)
            {
                var ai = Instantiate(blueprint.AI, this.CachedTransform);
                ai.transform.localPosition = Vector3.zero;
                ai.transform.localRotation = Quaternion.identity;
            }
            
            HK.Framework.EventSystems.Broker.Global.Receive<Died>()
                .Where(x => x.Character == this)
                .Take(1)
                .SubscribeWithState2(this, blueprint, (_, _this, b) =>
                {
                    Destroy(_this.gameObject);
                    b.DiedEffect
                        .Rent(_this.CachedTransform.position, _this.CachedTransform.rotation)
                        .ReturnToPoolOnTimer(3.0f);
                })
                .AddTo(this);
        }
    }
}
