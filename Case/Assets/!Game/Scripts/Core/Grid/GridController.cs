using _Game.Scripts.Data;
using _Game.Scripts.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.Core.Grid
{
    public class GridController : MonoBehaviour, IGridProvider
    {
        [Title("Settings")]
        [SerializeField] private ProductData productData;
        [SerializeField] private GridData gridData;
        
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

            var gridWidth  = _currentLevelData.GridMatrix.GetLength(0);
            var gridHeight = _currentLevelData.GridMatrix.GetLength(1);

            var offsetX = (gridWidth - 1) * gridData.cellSize / 2f;
            var offsetZ = (gridHeight - 1) * gridData.cellSize / 2f;

            for (int y = 0; y < gridHeight; y++)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    var productColor = _currentLevelData.GridMatrix[x, y];
                    if (productColor == ProductColor.Null) continue;

                    Vector3 position = new Vector3(x * gridData.cellSize - offsetX, 0, -y * gridData.cellSize + offsetZ);
                    var product = Instantiate(productData.productPrefab, gridData.gridContainer.position + position,
                        Quaternion.identity, gridData.gridContainer);
                    product.name = $"Product_{x},{y}_{productColor}";

                    var material = productData.GetMaterial(productColor);
                    product.SetMaterial(material);
                }
            }
        }

        public void ClearGrid()
        {
            if (gridData.gridContainer == null) return;

            for (int i = gridData.gridContainer.childCount - 1; i >= 0; i--)
                DestroyImmediate(gridData.gridContainer.GetChild(i).gameObject);
        }

        
    }
}
