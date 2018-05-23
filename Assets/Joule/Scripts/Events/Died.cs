using HK.Framework.EventSystems;
using Joule.CharacterControllers;

namespace Joule.Events.CharacterControllers
{
    /// <summary>
    /// <see cref="Character"/>が死亡した際のイベント
    /// </summary>
    public sealed class Died : Message<Died, Character>
    {
        /// <summary>
        /// 死亡したキャラクター
        /// </summary>
        public Character Character
        {
            get { return this.param1; }
        }
    }
}
