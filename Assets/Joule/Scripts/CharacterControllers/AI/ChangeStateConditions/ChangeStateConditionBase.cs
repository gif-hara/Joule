using UnityEngine;
using UnityEngine.Assertions;

namespace Joule.CharacterControllers.AI
{
    /// <summary>
    /// ステートを切り替える条件を持つ抽象クラス
    /// </summary>
    public abstract class ChangeStateConditionBase : ScriptableObject
    {
        public abstract bool Evalution(StateMachineBase stateMachine);

        public abstract ChangeStateConditionBase Clone { get; }
    }
}
