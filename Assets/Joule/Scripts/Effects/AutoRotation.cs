using UnityEngine;
using UnityEngine.Assertions;

namespace Joule.Effects
{
    /// <summary>
    /// アタッチしたオブジェクトを回転させる
    /// </summary>
    public sealed class AutoRotation : MonoBehaviour
    {
        public enum SimulationSpace
        {
            Local,
            World,
        }
        
        [SerializeField]
        private float speed;

        [SerializeField]
        private Vector3 axis;

        [SerializeField]
        private SimulationSpace simulationSpace;
        
        private Transform cachedTransform;

        void Awake()
        {
            this.cachedTransform = this.transform;
        }
        
        void Update()
        {
            var t = this.speed * Time.deltaTime;
            var value = Quaternion.AngleAxis(t, this.axis);
            if (this.simulationSpace == SimulationSpace.Local)
            {
                this.cachedTransform.localRotation *= value;
            }
            else if (this.simulationSpace == SimulationSpace.World)
            {
                this.cachedTransform.rotation *= value;
            }
            else
            {
                Assert.IsTrue(false);
            }
        }
    }
}
