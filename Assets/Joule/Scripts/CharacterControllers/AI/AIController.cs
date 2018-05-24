using UnityEngine;
using UnityEngine.Assertions;

namespace Joule.CharacterControllers.AI
{
    /// <summary>
    /// AIを制御するクラス
    /// </summary>
    [RequireComponent(typeof(Character))]
    public sealed class AIController : MonoBehaviour
    {
        [SerializeField]
        private StateBase state;

        private Character character;

        void Awake()
        {
            this.character = this.GetComponent<Character>();
            this.state = this.state.Clone;
            this.state.OnEnter(this.character);
        }
    }
}
