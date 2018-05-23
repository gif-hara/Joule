using System;
using Joule.GameSystems;
using UnityEngine;
using UnityEngine.Assertions;

namespace Joule
{
    /// <summary>
    /// Enumの拡張クラス
    /// </summary>
    public static partial class Extensions
    {
        public static string ToButtonNames(this ButtonNameType self)
        {
            switch (self)
            {
                case ButtonNameType.MoveHorizontal:
                    return ButtonNames.MoveHorizontal;
                case ButtonNameType.MoveVertical:
                    return ButtonNames.MoveVertical;
                case ButtonNameType.CameraHorizontal:
                    return ButtonNames.CameraHorizontal;
                case ButtonNameType.CameraVertical:
                    return ButtonNames.CameraVertical;
                case ButtonNameType.Fire:
                    return ButtonNames.Fire;
                case ButtonNameType.Lockon:
                    return ButtonNames.Lockon;
                case ButtonNameType.Skill0:
                    return ButtonNames.Skill0;
                case ButtonNameType.Skill1:
                    return ButtonNames.Skill1;
                case ButtonNameType.Skill2:
                    return ButtonNames.Skill2;
                default:
                    Assert.IsTrue(false, string.Format("未対応の値です {0}", self));
                    return String.Empty;
            }
        }
    }
}
