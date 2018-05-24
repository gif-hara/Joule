using System;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace Joule.CharacterControllers.AI
{
    /// <summary>
    /// ランダムに移動するステート
    /// </summary>
    [CreateAssetMenu(menuName = "Joule/AI/RandomMove")]
    public sealed class RandomMove : StateBase
    {
        [SerializeField]
        private float waitMin;
        
        [SerializeField]
        private float waitMax;

        [SerializeField]
        private float range;

        [SerializeField]
        private float speed;

        [SerializeField]
        private float destinationCheckDistance;
        
        public override void OnEnter(Character character)
        {
            var navMeshAgent = character.GetComponent<NavMeshAgent>();
            Assert.IsNotNull(navMeshAgent, string.Format("{0}に{1}がアタッチされていませんでした", character.name, typeof(NavMeshAgent)));

            var navMeshObstacle = character.GetComponent<NavMeshObstacle>();
            Assert.IsNotNull(navMeshAgent, string.Format("{0}に{1}がアタッチされていませんでした", character.name, typeof(NavMeshObstacle)));
            
            navMeshAgent.speed = this.speed;

            this.DoMove(navMeshAgent, navMeshObstacle);
        }

        public override StateBase Clone
        {
            get
            {
                var clone = CreateInstance<RandomMove>();
                clone.waitMin = this.waitMin;
                clone.waitMax = this.waitMax;
                clone.range = this.range;
                clone.speed = this.speed;
                clone.destinationCheckDistance = this.destinationCheckDistance;
                return clone;
            }
        }

        private void DoMove(NavMeshAgent navMeshAgent, NavMeshObstacle navMeshObstacle)
        {
            navMeshAgent.enabled = false;
            navMeshObstacle.enabled = true;
            Observable.Timer(TimeSpan.FromSeconds(Random.Range(this.waitMin, this.waitMax)))
                .SubscribeWithState3(this, navMeshAgent, navMeshObstacle, (_, _this, a, o) =>
                {
                    o.enabled = false;
                    a.enabled = true;
                    a.destination = navMeshAgent.transform.position +
                                    new Vector3(
                                        Random.Range(-this.range, this.range),
                                        0.0f,
                                        Random.Range(-this.range, this.range)
                                    );
                    _this.RestartDoMove(a, o);
                })
                .AddTo(this.runningEvents)
                .AddTo(navMeshAgent);
        }

        private void RestartDoMove(NavMeshAgent navMeshAgent, NavMeshObstacle navMeshObstacle)
        {
            navMeshAgent.UpdateAsObservable()
                .Where(_ => (navMeshAgent.transform.position - navMeshAgent.destination).sqrMagnitude < (this.destinationCheckDistance * this.destinationCheckDistance))
                .Take(1)
                .SubscribeWithState3(this, navMeshAgent, navMeshObstacle, (_, _this, a, o) => { _this.DoMove(a, o); })
                .AddTo(this.runningEvents)
                .AddTo(navMeshAgent);
        }
    }
}
