using _Game.Scripts.Core.Cubes;
using _Game.Scripts.Data;
using _Game.Scripts.Enums;

namespace _Game.Scripts.Events
{
    public readonly struct LevelLoadedEvent : IEvent
    {
        public readonly LevelData LevelData;
        public readonly int LevelNumber;
        public LevelLoadedEvent(LevelData levelData, int levelNumber)
        {
            LevelData = levelData;
            LevelNumber = levelNumber;
        }
    }

    public readonly struct CubeClickedEvent : IEvent
    {
        public readonly Cube Cube;
        public CubeClickedEvent(Cube cube) { Cube = cube; }
    }

    public readonly struct CubeDestroyedEvent : IEvent
    {
        public readonly Cube Cube;
        public readonly CubeState PreviousState;
        public CubeDestroyedEvent(Cube cube, CubeState previousState)
        {
            Cube = cube;
            PreviousState = previousState;
        }
    }

    public readonly struct GameWinEvent : IEvent { }
    
    public readonly struct GameLoseEvent : IEvent { }

    public readonly struct ConveyorCapacityChangedEvent : IEvent
    {
        public readonly int Current;
        public readonly int Max;
        public ConveyorCapacityChangedEvent(int current, int max)
        {
            Current = current;
            Max = max;
        }
    }
}