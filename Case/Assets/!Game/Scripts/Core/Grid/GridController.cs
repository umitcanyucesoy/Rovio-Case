using _Game.Scripts.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.Core.Grid
{
    public class GridController : MonoBehaviour, IGridProvider
    {
        [Title("Settings")] 
        [SerializeField] private Product productPrefab;
        [SerializeField] private Transform gridContainer;
        [SerializeField] private float cellSize = 1f;
        
        private LevelData _currentLevelData;
        
        public void InitGrid(LevelData levelData)
        {
            _currentLevelData = levelData;
            GenerateGrid();
        }

        private void GenerateGrid()
        {
            ClearGrid();
            if (_currentLevelData?.GridMatrix == null) return;
            
            
            int rows = _currentLevelData.GridMatrix.GetLength(0);
            int cols = _currentLevelData.GridMatrix.GetLength(1);

            float offsetX = (cols - 1) * cellSize / 2f;
            float offsetZ = (rows - 1) * cellSize / 2f;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    if (!_currentLevelData.GridMatrix[row, col]) continue;

                    Vector3 position = new Vector3(
                        col * cellSize - offsetX,
                        0,
                        row * cellSize - offsetZ
                    );

                    var product = Instantiate(productPrefab, gridContainer.position + position,
                        Quaternion.identity, gridContainer);
                    product.name = $"Product_R{row}_C{col}";
                }
            }
        }

        public void ClearGrid()
        {
            if (gridContainer == null) return;

            for (int i = gridContainer.childCount - 1; i >= 0; i--)
                DestroyImmediate(gridContainer.GetChild(i).gameObject);
        }
    }
}