using HK.Framework.EventSystems;
using Joule.CameraControllers;
using Joule.Events.CharacterControllers;
using UnityEngine;
using UnityEngine.Assertions;

namespace Joule.CharacterControllers
{
    /// <summary>
    /// プレイヤーを生成するクラス
    /// </summary>
    public sealed class PlayerSpawner : MonoBehaviour
    {
        [SerializeField]
        private Character character;

        [SerializeField]
        private GameObject model;

        [SerializeField]
        private PlayerMuzzleController muzzleController;

        [SerializeField]
        private GameCameraController cameraController;

        void Awake()
        {
            var t = this.transform;
            var character = Instantiate(this.character);
            Instantiate(this.model, character.CachedTransform);
            Instantiate(this.muzzleController, character.CachedTransform).Attach(character);
            Instantiate(this.cameraController);
            Broker.Global.Publish(PlayerSpawned.Get(character));
        }
    }
}
