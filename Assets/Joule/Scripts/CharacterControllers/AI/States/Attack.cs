using System;
using Joule.BulletControllers;
using UniRx;
using UniRx.Triggers;
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

        [SerializeField]
        private float interval;

        [SerializeField]
        private float rotationSmoothTime;

        [SerializeField]
        private bool enterOnAttack;

        private MuzzleController[] muzzleControllers;

        private Vector3 rotationVelocity;
        
        public override void OnEnter(AIControllerBase aiController)
        {
            var navMeshAgent = aiController.Owner.GetComponent<NavMeshAgent>();
            Assert.IsNotNull(navMeshAgent, string.Format("{0}に{1}がアタッチされていませんでした", aiController.Owner, typeof(NavMeshAgent)));

            var navMeshObstacle = aiController.Owner.GetComponent<NavMeshObstacle>();
            Assert.IsNotNull(navMeshObstacle, string.Format("{0}に{1}がアタッチされていませんでした", aiController.Owner, typeof(NavMeshObstacle)));

            navMeshAgent.enabled = false;
            navMeshObstacle.enabled = true;

            aiController.UpdateAsObservable()
                .Where(_ => aiController.isActiveAndEnabled)
                .SubscribeWithState2(this, aiController, (_, _this, a) =>
                {
                    var owner = a.Owner.CachedTransform;
                    var v = a.Target.CachedTransform.position - owner.position;
                    v.y = 0.0f;
                    var current = owner.rotation.eulerAngles;
                    var target = Quaternion.LookRotation(v.normalized).eulerAngles;
                    var t = _this.rotationSmoothTime * Time.deltaTime;
                    owner.rotation = Quaternion.Euler(
                        Mathf.SmoothDampAngle(current.x, target.x, ref _this.rotationVelocity.x, t),
                        Mathf.SmoothDampAngle(current.y, target.y, ref _this.rotationVelocity.y, t),
                        Mathf.SmoothDampAngle(current.z, target.z, ref _this.rotationVelocity.z, t)
                    );
                })
                .AddTo(this.runningEvents)
                .AddTo(aiController);

            Observable.Interval(TimeSpan.FromSeconds(this.interval))
                .Where(_ => aiController.isActiveAndEnabled)
                .SubscribeWithState2(this, aiController, (_, _this, a) =>
                {
                    _this.Fire(a.Owner);
                })
                .AddTo(this.runningEvents)
                .AddTo(aiController);

            if (this.enterOnAttack)
            {
                this.Fire(aiController.Owner);
            }
        }

        public override StateBase Clone(AIControllerBase aiController)
        {
            var instance = CreateInstance<Attack>();
            instance.muzzle = Instantiate(this.muzzle);
            instance.muzzle.transform.SetParent(aiController.Owner.CachedTransform);
            instance.muzzle.transform.localPosition = Vector3.zero;
            instance.muzzle.transform.localRotation = Quaternion.identity;
            instance.interval = this.interval;
            instance.rotationSmoothTime = this.rotationSmoothTime;
            instance.enterOnAttack = this.enterOnAttack;
            instance.muzzleControllers = instance.muzzle.GetComponentsInChildren<MuzzleController>();
            Assert.AreNotEqual(instance.muzzleControllers.Length, 0, string.Format("{0}に{1}が存在しません", instance.muzzle, typeof(MuzzleController)));

            return instance;
        }

        private void Fire(Character owner)
        {
            foreach (var m in this.muzzleControllers)
            {
                m.Fire(owner);
            }
        }
    }
}
