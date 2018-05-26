using System;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace Joule.CharacterControllers.AI
{
    /// <summary>
    /// <see cref="Character"/>を生成するステート
    /// </summary>
    [CreateAssetMenu(menuName = "Joule/AI/States/Spawn")]
    public sealed class Spawn : StateBase
    {
        [SerializeField]
        private float interval;
        
        [SerializeField]
        private SpawnRegulation[] regulations;
        
        public override void OnEnter(AIControllerBase aiController)
        {
            Observable.Interval(TimeSpan.FromSeconds(this.interval))
                .Where(_ => aiController.isActiveAndEnabled)
                .SubscribeWithState(this, (_, _this) =>
                {
                    foreach (var spawnRegulation in _this.regulations)
                    {
                        spawnRegulation.Spawn();
                    }
                })
                .AddTo(this.runningEvents)
                .AddTo(aiController);
        }

        public override StateBase Clone(AIControllerBase aiController)
        {
            var instance = CreateInstance<Spawn>();
            instance.interval = this.interval;
            instance.regulations = new SpawnRegulation[this.regulations.Length];
            for (var i = 0; i < this.regulations.Length; i++)
            {
                instance.regulations[i] = this.regulations[i].Clone(aiController);
            }

            return instance;
        }
        
        [Serializable]
        public class SpawnRegulation
        {
            public CharacterSpawner Spawner;

            public float RandomRange;

            public SpawnRegulation Clone(AIControllerBase aiController)
            {
                var instance = new SpawnRegulation
                {
                    Spawner = Instantiate(this.Spawner, aiController.Owner.CachedTransform),
                    RandomRange = this.RandomRange
                };

                instance.Spawner.transform.localPosition = Vector3.zero;
                return instance;
            }

            public Character Spawn()
            {
                var r = new Vector3(
                    Random.Range(-this.RandomRange, this.RandomRange),
                    0.0f,
                    Random.Range(-this.RandomRange, this.RandomRange)
                    );
                return this.Spawner.Spawn(this.Spawner.transform.position + r);
            }
        }
    }
}
