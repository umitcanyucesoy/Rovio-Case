using _Game.Scripts.Data;
using _Game.Scripts.Enums;
using _Game.Scripts.Services;
using UnityEngine;

namespace _Game.Scripts.Core.Grid
{
    public interface IGridService : IService
    {
        public void InitGrid(LevelData levelData);
        public Product FindAndRemoveMatchingProduct(int column, ProductColor color, bool reverse = false);
        public Product FindAndRemoveMatchingProductByRow(int row, ProductColor color, bool reverse = false);
        public Vector2Int GetGridDimensions();
        public float GetCellSize();
        public Transform GetGridRoot();
    }
}
