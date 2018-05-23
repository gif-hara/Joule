using HK.Framework.EventSystems;
using Joule.Events.CharacterControllers;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace Joule.CharacterControllers
{
    /// <summary>
    /// <see cref="Character"/>を生成する
    /// </summary>
    [ExecuteInEditMode]
    public sealed class CharacterSpawner : MonoBehaviour
    {
        [SerializeField]
        private Character characterPrefab;

        [SerializeField]
        private GameObject modelPrefab;
        
        [SerializeField]
        private bool awakeOnSpawn;

        void Awake()
        {
#if UNITY_EDITOR
            if(!Application.isPlaying)
            {
                return;
            }
#endif
            
            if (!this.awakeOnSpawn)
            {
                return;
            }
            
            this.Spawn();
        }

        private void Spawn()
        {
            var t = this.transform;
            var character = Instantiate(this.characterPrefab);
            character.CachedTransform.position = t.position;
            character.CachedTransform.rotation = t.rotation;
            Instantiate(this.modelPrefab, character.CachedTransform);
            Broker.Global.Publish(CharacterSpawned.Get(character));
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

            if(this.modelPrefab == null)
            {
                return;
            }

            this.imageModel = Instantiate(this.modelPrefab, this.transform);
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
