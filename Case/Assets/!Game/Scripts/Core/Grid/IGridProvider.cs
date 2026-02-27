using _Game.Scripts.Data;

namespace _Game.Scripts.Core.Grid
{
    public interface IGridProvider
    {
        public void InitGrid(LevelData levelData);
        public void ClearGrid();
    }
}