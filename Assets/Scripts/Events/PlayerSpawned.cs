using HK.Framework.EventSystems;
using Joule.CharacterControllers;

namespace Joule.Events.CharacterControllers
{
    /// <summary>
    /// プレイヤーが生成された際のイベント
    /// </summary>
    public sealed class PlayerSpawned : Message<PlayerSpawned, Character>
    {
        public Character Player
        {
            get { return this.param1; }
        }
    }
}
