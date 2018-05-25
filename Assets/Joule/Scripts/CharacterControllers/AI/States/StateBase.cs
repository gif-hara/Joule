using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace Joule.CharacterControllers.AI
{
    /// <summary>
    /// ステートクラスの抽象クラス
    /// </summary>
    public abstract class StateBase : ScriptableObject
    {
        protected readonly CompositeDisposable runningEvents = new CompositeDisposable();
        
        public abstract void OnEnter(AIControllerBase aiController);

        public virtual void OnExit()
        {
            this.runningEvents.Clear();
        }

        public abstract StateBase Clone { get; }
    }
}
