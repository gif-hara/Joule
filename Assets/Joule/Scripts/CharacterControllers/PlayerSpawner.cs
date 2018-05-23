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
    public sealed class PlayerSpawner : MonoBehaviour
    {
        [SerializeField]
        private Character character;

        [SerializeField]
        private GameObject model;

        [SerializeField]
        private MuzzleRegulation[] muzzleRegulations;

        [SerializeField]
        private GameCameraController cameraController;

        void Awake()
        {
#if UNITY_EDITOR
            if(!Application.isPlaying)
            {
                return;
            }
#endif
            var t = this.transform;
            var character = Instantiate(this.character);
            character.CachedTransform.position = t.position;
            character.CachedTransform.rotation = t.rotation;
            Instantiate(this.model, character.CachedTransform);
            foreach (var muzzleRegulation in this.muzzleRegulations)
            {
                muzzleRegulation.Instantiate(character);
            }
            Instantiate(this.cameraController);
            Broker.Global.Publish(PlayerSpawned.Get(character));
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

#if UNITY_EDITOR
        [SerializeField][HideInInspector]
        private GameObject imageModel;

        private void Start()
        {
            EditorApplication.playModeStateChanged -= this.PlayModeStateChanged;
            EditorApplication.playModeStateChanged += this.PlayModeStateChanged;
        }

        private void Update()
        {
            if (Application.isPlaying)
            {
                return;
            }

            if (this.imageModel != null)
            {
                return;
            }

            if(this.model == null)
            {
                return;
            }

            this.imageModel = Instantiate(this.model, this.transform);
            this.imageModel.transform.localPosition = Vector3.zero;
            this.imageModel.transform.localRotation = Quaternion.identity;
            this.imageModel.hideFlags = HideFlags.DontSave | HideFlags.NotEditable;
        }

        private void PlayModeStateChanged(PlayModeStateChange state)
        {
            if(state == PlayModeStateChange.EnteredPlayMode)
            {
                DestroyImmediate(this.imageModel);
            }
        }
#endif
    }
}
