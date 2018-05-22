using HK.Framework.EventSystems;
using Joule.CameraControllers;
using Joule.Events.CharacterControllers;
using UnityEngine;
using UnityEngine.Assertions;
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
        private PlayerMuzzleController muzzleController;

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
            Instantiate(this.muzzleController, character.CachedTransform).Attach(character);
            Instantiate(this.cameraController);
            Broker.Global.Publish(PlayerSpawned.Get(character));
        }

#if UNITY_EDITOR
        [SerializeField]
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
