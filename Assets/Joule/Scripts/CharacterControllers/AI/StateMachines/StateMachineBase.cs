using UnityEngine;

namespace Joule.CharacterControllers.AI
{
    /// <summary>
    /// ステートマシンの抽象クラス
    /// </summary>
    public abstract class StateMachineBase : ScriptableObject
    {
        public AIController AIController { get; protected set; }

        public StateBase CurrentState { get; private set; }

        protected int currentStateIndex;

        public abstract StateMachineBase Clone(AIController aiController);

        public void Change(StateBase nextState, int stateIndex)
        {
            if (this.CurrentState != null)
            {
                this.CurrentState.OnExit();
            }

            this.CurrentState = nextState;
            this.CurrentState.OnEnter(this.AIController.Owner);
            this.currentStateIndex = stateIndex;
        }
    }
}
