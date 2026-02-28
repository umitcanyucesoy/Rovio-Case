using _Game.Scripts.Data;
using UnityEngine;

namespace _Game.Scripts.Core.Grid
{
    public interface IGridProvider
    {
        public void InitGrid(LevelData levelData);
        public void ClearGrid();
    }
}