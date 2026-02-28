using _Game.Scripts.Data;

namespace _Game.Scripts.Events
{
    public readonly struct LevelLoadedEvent : IEvent
    {
        public readonly LevelData LevelData;
        
        public LevelLoadedEvent(LevelData levelData)
        {
            LevelData = levelData;
        }
    }
}