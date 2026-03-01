using _Game.Scripts.Core.Cubes;
using _Game.Scripts.Data;

namespace _Game.Scripts.Events
{
    public readonly struct LevelLoadedEvent : IEvent
    {
        public readonly LevelData LevelData;
        public LevelLoadedEvent(LevelData levelData) { LevelData = levelData; }
    }

    public readonly struct CubeClickedEvent : IEvent
    {
        public readonly Cube Cube;
        public CubeClickedEvent(Cube cube) { Cube = cube; }
    }

    public readonly struct CubeDestroyedEvent : IEvent
    {
        public readonly Cube Cube;
        public CubeDestroyedEvent(Cube cube) { Cube = cube; }
    }

    public readonly struct GameWinEvent : IEvent { }
    
    public readonly struct GameLoseEvent : IEvent { }
}