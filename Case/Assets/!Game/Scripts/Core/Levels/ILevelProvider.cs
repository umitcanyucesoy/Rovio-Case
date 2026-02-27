using _Game.Scripts.Core.Grid;

namespace _Game.Scripts.Core.Levels
{
    public interface ILevelProvider
    {
        public void Init(IGridProvider gridProvider);
        public void LoadLevel();
        public void NextLevel();
    }
}