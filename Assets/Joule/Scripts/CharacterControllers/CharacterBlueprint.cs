using Joule.ObjectPools;
using UnityEngine;
using UnityEngine.Assertions;

namespace Joule.CharacterControllers
{
    /// <summary>
    /// 
    /// </summary>
    [CreateAssetMenu(menuName = "Joule/Character/Blueprint")]
    public sealed class CharacterBlueprint : ScriptableObject
    {
        [SerializeField]
        private Character character;
        
        [SerializeField]
        private GameObject model;
        public GameObject Model
        {
            get { return model; }
        }

        [SerializeField]
        private PoolableEffect diedEffect;
        public PoolableEffect DiedEffect
        {
            get { return diedEffect; }
        }

        [SerializeField]
        private CharacterStatus status;
        public CharacterStatus Status
        {
            get { return status; }
        }

        public Character Instantiate(Vector3 position, Quaternion rotation)
        {
            var c = Instantiate(this.character, position, rotation);
            c.Initialize(this);

            return c;
        }
    }
}
