using UnityEngine;
using UnityEngine.Assertions;

namespace Joule.GameSystems
{
    /// <summary>
    /// 三人称視点に関するユーティリティ
    /// </summary>
    public static class ThirdPersonUtility
    {
        public static Vector3 Forward
        {
            get
            {
                var vector = Camera.transform.forward;
                vector.y = 0.0f;

                return vector;
            }
        }

        public static Vector3 Right
        {
            get
            {
                var vector = Camera.transform.right;
                vector.y = 0.0f;

                return vector;
            }
        }

        private static Camera Camera
        {
            get { return Camera.main; }
        }
    }
}
