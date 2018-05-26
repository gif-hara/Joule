using UnityEngine;
using UnityEngine.Assertions;

namespace Joule.CharacterControllers.AI
{
    /// <summary>
    /// <see cref="AIControllerBase.Target"/>が存在しているかを条件とするクラス
    /// </summary>
    [CreateAssetMenu(menuName = "Joule/AI/Conditions/ExistTarget")]
    public sealed class ExistTarget : ChangeStateConditionBase
    {
        [SerializeField]
        private bool isExist = true;
        
        public override bool Evalution(AIControllerBase aiController)
        {
            return (aiController.Target != null) == this.isExist;
        }

        public override ChangeStateConditionBase Clone
        {
            get
            {
                var instance = CreateInstance<ExistTarget>();
                instance.isExist = this.isExist;
                return instance;
            }
        }
    }
}
