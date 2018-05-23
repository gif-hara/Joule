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
        private CharacterBlueprint blueprint;
                
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
            var character = this.blueprint.Instantiate(t.position, t.rotation);
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

            if(this.blueprint == null)
            {
                return;
            }

            this.imageModel = Instantiate(this.blueprint.Model, this.transform);
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
