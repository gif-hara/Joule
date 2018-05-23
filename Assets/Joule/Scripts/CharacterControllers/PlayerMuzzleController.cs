using Joule.BulletControllers;
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
        private float coolTime;
        
        [SerializeField]
        private MuzzleController[] muzzleControllers;

        private float currentCoolTime;

        void Awake()
        {
            this.currentCoolTime = this.coolTime;
        }

        void Update()
        {
            if (Input.GetButtonDown(ButtonNames.Fire) && this.CanFire)
            {
                foreach (var muzzleController in this.muzzleControllers)
                {
                    muzzleController.Fire(this.character);
                }

                this.currentCoolTime = 0.0f;
            }

            this.currentCoolTime += Time.deltaTime;
        }

        public void Attach(Character character)
        {
            this.character = character;
        }

        private bool CanFire
        {
            get { return this.currentCoolTime >= this.coolTime; }
        }
    }
}
