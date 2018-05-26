using System;
using HK.Framework.EventSystems;
using Joule.CameraControllers;
using Joule.Events.CharacterControllers;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Joule.CharacterControllers
{
    /// <summary>
    /// プレイヤーを生成するクラス
    /// </summary>
    [ExecuteInEditMode]
    public sealed class PlayerSpawner : CharacterSpawner
    {
        [SerializeField]
        private MuzzleRegulation[] muzzleRegulations;

        [SerializeField]
        private GameCameraController cameraController;

        public override Character Spawn()
        {
            var character = base.Spawn();
            foreach (var muzzleRegulation in this.muzzleRegulations)
            {
                muzzleRegulation.Instantiate(character);
            }
            Instantiate(this.cameraController);
            Broker.Global.Publish(PlayerSpawned.Get(character));

            return character;
        }

        [Serializable]
        public class MuzzleRegulation
        {
            [SerializeField]
            private PlayerMuzzleController prefab;

            [SerializeField]
            private PlayerFireCondition condition;

            public PlayerMuzzleController Instantiate(Character owner)
            {
                var controller = Object.Instantiate(this.prefab, owner.CachedTransform);
                controller.transform.localPosition = Vector3.zero;
                controller.transform.localRotation = Quaternion.identity;
                controller.Attach(owner, this.condition);

                return controller;
            }
        }
    }
}
