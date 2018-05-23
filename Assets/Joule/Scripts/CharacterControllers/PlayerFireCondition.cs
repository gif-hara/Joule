using UnityEngine;

namespace Joule.CharacterControllers
{
    /// <summary>
    /// プレイヤーの攻撃を行う条件を持つ抽象クラス
    /// </summary>
    public abstract class PlayerFireCondition : ScriptableObject
    {
        public abstract bool Condition { get; }
    }
}
