using UnityEngine;
using UnityEngine.Assertions;

namespace Joule.CharacterControllers.AI
{
    /// <summary>
    /// AIを制御するクラス
    /// </summary>
    public abstract class AIControllerBase : MonoBehaviour
    {
        public Character Owner { get; private set; }

        public Character Target { get; set; }

        public StateBase CurrentState { get; private set; }

        protected int currentStateIndex;

        protected abstract void InternalAwake();

        void Awake()
        {
            this.Owner = this.GetComponentInParent<Character>();
            Assert.IsNotNull(this.Owner);
            this.InternalAwake();
        }
        
        public void Change(StateBase nextState, int stateIndex)
        {
            if (this.CurrentState != null)
            {
                this.CurrentState.OnExit();
            }

            this.CurrentState = nextState;
            this.CurrentState.OnEnter(this.Owner);
            this.currentStateIndex = stateIndex;
        }
    }
}
