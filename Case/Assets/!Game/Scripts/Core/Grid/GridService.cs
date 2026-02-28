using _Game.Scripts.Data;
using _Game.Scripts.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.Core.Grid
{
    public class GridService : MonoBehaviour, IGridService
    {
        [Title("Settings")]
        [SerializeField] private ProductData productData;
        [SerializeField] private GridData gridData;
        
        private LevelData _currentLevelData;
        private Product[,] _gridState;
        private int _gridWidth;
        private int _gridHeight;
        
        public Vector2Int GetGridDimensions() => new(_gridWidth, _gridHeight);
        public float GetCellSize() => gridData.cellSize;
        public Transform GetGridRoot() => gridData.gridContainer;

        public void InitGrid(LevelData levelData)
        {
            _currentLevelData = levelData;
            GenerateGrid();
        }

        private void GenerateGrid()
        {
            ClearGrid();
            if (_currentLevelData?.GridMatrix == null) return;

            _gridWidth  = _currentLevelData.GridMatrix.GetLength(0);
            _gridHeight = _currentLevelData.GridMatrix.GetLength(1);
            _gridState = new Product[_gridWidth, _gridHeight];

            var offsetX = (_gridWidth - 1) * gridData.cellSize / 2f;
            var offsetZ = (_gridHeight - 1) * gridData.cellSize / 2f;

            for (int y = 0; y < _gridHeight; y++)
            {
                for (int x = 0; x < _gridWidth; x++)
                {
                    var productColor = _currentLevelData.GridMatrix[x, y];
                    if (productColor == ProductColor.Null) continue;

                    Vector3 position = new Vector3(x * gridData.cellSize - offsetX, 0, -y * gridData.cellSize + offsetZ);
                    var product = Instantiate(productData.productPrefab, gridData.gridContainer.position + position,
                        Quaternion.identity, gridData.gridContainer);
                    product.name = $"Product_{x},{y}_{productColor}";

                    var material = productData.GetMaterial(productColor);
                    product.SetColor(productColor, material);
                    
                    _gridState[x, y] = product;
                }
            }
        }

        public Product FindAndRemoveMatchingProduct(int column, ProductColor color, bool reverse = false)
        {
            if (_gridState == null) return null;
            if (column < 0 || column >= _gridWidth) return null;

            var start = reverse ? _gridHeight - 1 : 0;
            var end   = reverse ? -1 : _gridHeight;
            var step  = reverse ? -1 : 1;

            for (var y = start; y != end; y += step)
            {
                var product = _gridState[column, y];
                if (product && product.Color == color)
                {
                    _gridState[column, y] = null;
                    return product;
                }
            }

            return null;
        }

        public Product FindAndRemoveMatchingProductByRow(int row, ProductColor color, bool reverse = false)
        {
            if (_gridState == null) return null;
            if (row < 0 || row >= _gridHeight) return null;

            var start = reverse ? _gridWidth - 1 : 0;
            var end   = reverse ? -1 : _gridWidth;
            var step  = reverse ? -1 : 1;

            for (var x = start; x != end; x += step)
            {
                var product = _gridState[x, row];
                if (product && product.Color == color)
                {
                    _gridState[x, row] = null;
                    return product;
                }
            }

            return null;
        }
        
        public void ClearGrid()
        {
            _gridState = null;
            for (int i = gridData.gridContainer.childCount - 1; i >= 0; i--)
                DestroyImmediate(gridData.gridContainer.GetChild(i).gameObject);
        }
    }
}
