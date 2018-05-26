using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

namespace Joule.CharacterControllers.AI
{
    /// <summary>
    /// <see cref="AIControllerBase.Target"/>に対して攻撃を行うステート
    /// </summary>
    [CreateAssetMenu(menuName = "Joule/AI/States/Attack")]
    public sealed class Attack : StateBase
    {
        [SerializeField]
        private GameObject muzzle;
        
        public override void OnEnter(AIControllerBase aiController)
        {
            var navMeshAgent = aiController.Owner.GetComponent<NavMeshAgent>();
            Assert.IsNotNull(navMeshAgent, string.Format("{0}に{1}がアタッチされていませんでした", aiController.Owner, typeof(NavMeshAgent)));

            var navMeshObstacle = aiController.Owner.GetComponent<NavMeshObstacle>();
            Assert.IsNotNull(navMeshObstacle, string.Format("{0}に{1}がアタッチされていませんでした", aiController.Owner, typeof(NavMeshObstacle)));

            navMeshAgent.enabled = false;
            navMeshObstacle.enabled = true;
            Debug.Log("?");
        }

        public override StateBase Clone(AIControllerBase aiController)
        {
            var instance = CreateInstance<Attack>();
            instance.muzzle = Instantiate(this.muzzle);
            instance.muzzle.transform.SetParent(aiController.Owner.CachedTransform);
            instance.muzzle.transform.localPosition = Vector3.zero;
            instance.muzzle.transform.localRotation = Quaternion.identity;
            return instance;
        }
    }
}
