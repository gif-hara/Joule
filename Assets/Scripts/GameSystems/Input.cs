using UnityEngine;
using UnityEngine.Assertions;

namespace Joule.GameSystems
{
    /// <summary>
    /// 入力に関するクラス
    /// </summary>
    public static class Input
    {
        public static float Horizontal
        {
            get { return UnityEngine.Input.GetAxis("Horizontal"); }
        }
        
        public static float Vertical
        {
            get { return UnityEngine.Input.GetAxis("Vertical"); }
        }
    }
}
