using Joule.GameSystems;
using UnityEngine;
using UnityEngine.Assertions;

namespace Joule.CharacterControllers
{
    /// <summary>
    /// プレイヤー用の銃口を制御するクラス
    /// </summary>
    public sealed class PlayerMuzzleController : MonoBehaviour
    {
        [SerializeField]
        private Character character;
        
        [SerializeField]
        private MuzzleController[] muzzleControllers;

        void Update()
        {
            if (Input.GetButtonDown(ButtonNames.Fire))
            {
                foreach (var muzzleController in this.muzzleControllers)
                {
                    muzzleController.Fire(this.character);
                }
            }
        }

        public void Attach(Character character)
        {
            this.character = character;
        }
    }
}
