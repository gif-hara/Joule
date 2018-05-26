using UnityEngine;
using UnityEngine.Assertions;

namespace Joule.CharacterControllers.AI
{
    /// <summary>
    /// <see cref="AIControllerBase.Target"/>との距離を条件とするクラス
    /// </summary>
    [CreateAssetMenu(menuName = "Joule/AI/Conditions/DistanceTarget")]
    public sealed class DistanceTarget : ChangeStateConditionBase
    {
        public enum EvaluteType
        {
            Greater,
            Less,
        }

        [SerializeField]
        private EvaluteType evaluteType;

        [SerializeField]
        private float distance;
        
        public override bool Evalution(AIControllerBase aiController)
        {
            Assert.IsNotNull(aiController.Target, string.Format("{0}のAIにTargetが設定されていません", aiController.Owner));
            var sqrMagnitude =
                (aiController.Owner.CachedTransform.position - aiController.Target.CachedTransform.position)
                .sqrMagnitude;
            var d = this.distance * this.distance;

            return this.evaluteType == EvaluteType.Greater
                ? sqrMagnitude > d
                : sqrMagnitude < d;
        }

        public override ChangeStateConditionBase Clone
        {
            get
            {
                var instance = CreateInstance<DistanceTarget>();
                instance.evaluteType = this.evaluteType;
                instance.distance = this.distance;
                return instance;
            }
        }
    }
}
