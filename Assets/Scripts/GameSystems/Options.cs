using UnityEngine;

namespace Joule.GameSystems
{
    /// <summary>
    /// オプションデータ
    /// </summary>
    [CreateAssetMenu(menuName = "Joule/Options")]
    public sealed class Options : ScriptableObject
    {
        [SerializeField]
        private OptionData data;
        public OptionData Data
        {
            get { return data; }
        }

        public static Options Instance { get; private set; }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoad()
        {
            Instance = Resources.Load<Options>("BootSystems/Options");
        }
    }
}
