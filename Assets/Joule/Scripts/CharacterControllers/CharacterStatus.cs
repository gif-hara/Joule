using System;

namespace Joule.CharacterControllers
{
    /// <summary>
    /// キャラクターのステータス
    /// </summary>
    [Serializable]
    public sealed class CharacterStatus
    {
        public int HitPoint;

        public CharacterStatus()
        {
            this.HitPoint = 1;
        }

        public CharacterStatus(CharacterStatus other)
        {
            this.HitPoint = other.HitPoint;
        }

        public bool IsDead
        {
            get { return this.HitPoint <= 0; }
        }
    }
}
