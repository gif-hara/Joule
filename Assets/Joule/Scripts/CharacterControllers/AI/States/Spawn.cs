using System;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace Joule.CharacterControllers.AI
{
    /// <summary>
    /// <see cref="Character"/>を生成するステート
    /// </summary>
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
                        spawnRegulation.Spawner.Spawn();
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

            public Vector3 Offset;

            public SpawnRegulation Clone(AIControllerBase aiController)
            {
                return new SpawnRegulation
                {
                    Spawner = Instantiate(this.Spawner, aiController.Owner.CachedTransform),
                    Offset = this.Offset
                };
            }
        }
    }
}
