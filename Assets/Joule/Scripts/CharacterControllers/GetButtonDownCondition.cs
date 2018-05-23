using Joule.CharacterControllers;
using Joule.GameSystems;
using UnityEngine;
using UnityEngine.Assertions;

namespace Joule.CharacterControllers
{
    /// <summary>
    /// <see cref="Input.GetButtonDown"/>によって攻撃を行える条件クラス
    /// </summary>
    [CreateAssetMenu(menuName = "Joule/PlayerFireCondition/GetButtonDown")]
    public sealed class GetButtonDownCondition : PlayerFireCondition
    {
        [SerializeField]
        private ButtonNameType type;
        
        public override bool Condition
        {
            get { return Input.GetButtonDown(type.ToButtonNames()); }
        }
    }
}
