using _Game.Scripts.Core.Conveyor;
using _Game.Scripts.Core.Cubes;
using _Game.Scripts.Core.Grid;

namespace _Game.Scripts.Core.Levels
{
    public interface ILevelProvider
    {
        public void Init(IGridProvider gridProvider, ICubeProvider cubeProvider);
        public void LoadLevel();
        public void NextLevel();
    }
}