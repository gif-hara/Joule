using System.Runtime.InteropServices;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

namespace Joule.CharacterControllers.AI
{
    /// <summary>
    /// <see cref="AIControllerBase.Target"/>を追いかけるステート
    /// </summary>
    [CreateAssetMenu(menuName = "Joule/AI/States/Chase")]
    public sealed class Chase : StateBase
    {
        [SerializeField]
        private float speed;
        
        public override void OnEnter(AIControllerBase aiController)
        {
            var navMeshAgent = aiController.Owner.GetComponent<NavMeshAgent>();
            Assert.IsNotNull(navMeshAgent, string.Format("{0}に{1}がアタッチされていませんでした", aiController.Owner, typeof(NavMeshAgent)));

            var navMeshObstacle = aiController.Owner.GetComponent<NavMeshObstacle>();
            Assert.IsNotNull(navMeshObstacle, string.Format("{0}に{1}がアタッチされていませんでした", aiController.Owner, typeof(NavMeshObstacle)));

            Assert.IsNotNull(aiController.Target, string.Format("{0}のAIにTargetが設定されていません", aiController.Owner));
            
            navMeshObstacle.enabled = false;
            navMeshAgent.enabled = true;
            navMeshAgent.speed = this.speed;

            aiController.UpdateAsObservable()
                .Where(_ => aiController.isActiveAndEnabled)
                .Where(_ => navMeshAgent.enabled)
                .SubscribeWithState2(navMeshAgent, aiController,
                    (_, n, a) =>
                    {
                        n.destination = a.Target.CachedTransform.position;
                    })
                .AddTo(this.runningEvents)
                .AddTo(aiController);
        }

        public override StateBase Clone(AIControllerBase aiController)
        {
            var instance = CreateInstance<Chase>();
            instance.speed = this.speed;
            return instance;
        }
    }
}
