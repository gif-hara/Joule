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
            navMeshAgent.destination = aiController.Target.CachedTransform.position;
        }

        public override StateBase Clone
        {
            get
            {
                var instance = CreateInstance<Chase>();
                instance.speed = this.speed;
                return instance;
            }
        }
    }
}
