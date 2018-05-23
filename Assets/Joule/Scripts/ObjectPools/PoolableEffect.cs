using System;
using HK.Framework;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace Joule.ObjectPools
{
    /// <summary>
    /// プール可能なエフェクト
    /// </summary>
    public sealed class PoolableEffect : MonoBehaviour
    {
        public static readonly ObjectPoolBundle<PoolableEffect> Bundle = new ObjectPoolBundle<PoolableEffect>();

        private ObjectPool<PoolableEffect> pool;

        private Transform cachedTransform;

        void Awake()
        {
            this.cachedTransform = this.transform;
        }

        public PoolableEffect Rent(Vector3 position, Quaternion rotation)
        {
            var pool = Bundle.Get(this);
            var instance = pool.Rent();
            instance.pool = pool;
            instance.cachedTransform.position = position;
            instance.cachedTransform.rotation = rotation;

            return instance;
        }

        public IDisposable ReturnToPoolOnTimer(float seconds)
        {
            return Observable.Timer(TimeSpan.FromSeconds(seconds))
                .SubscribeWithState(this, (_, _this) => { _this.pool.Return(_this); })
                .AddTo(this);
        }
    }
}
