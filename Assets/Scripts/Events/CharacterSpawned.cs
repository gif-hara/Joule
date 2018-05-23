using HK.Framework.EventSystems;
using Joule.CharacterControllers;

namespace Joule.Events.CharacterControllers
{
    /// <summary>
    /// <see cref="Character"/>が生成された際のイベント
    /// </summary>
    public sealed class CharacterSpawned : Message<CharacterSpawned, Character>
    {
        public Character Character
        {
            get { return this.param1; }
        }
    }
}
