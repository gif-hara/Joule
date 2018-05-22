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
        
        public IMessageBroker Broker { get; private set; }

        void Awake()
        {
            this.Broker = HK.Framework.EventSystems.Broker.GetGameObjectBroker(this.gameObject);
        }

        public void Initialize(CharacterStatus status)
        {
            this.Status = status;
        }
    }
}
