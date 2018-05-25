using UnityEngine;
using UnityEngine.Assertions;

namespace Joule.CharacterControllers.AI
{
    /// <summary>
    /// AIを制御するクラス
    /// </summary>
    [RequireComponent(typeof(Character))]
    public sealed class AIController : MonoBehaviour
    {
        public Character Owner { get; private set; }

        [SerializeField]
        private StateMachineBase stateMachineBase;
        
        public Character Target { get; set; }

        private StateMachineBase stateMachine;

        void Awake()
        {
            this.Owner = this.GetComponentInParent<Character>();
            this.stateMachine = this.stateMachineBase.Clone(this);
        }
    }
}
