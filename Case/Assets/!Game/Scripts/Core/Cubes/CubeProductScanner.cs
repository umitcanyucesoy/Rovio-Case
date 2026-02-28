using _Game.Scripts.Core.Grid;
using _Game.Scripts.Enums;
using _Game.Scripts.Services;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts.Core.Cubes
{
    [RequireComponent(typeof(Cube))]
    public class CubeProductScanner : MonoBehaviour
    {
        private Cube _cube;
        private ProductColor _targetColor;
        private Vector3 _gridOrigin;
        private float _cellSize;
        private int _gridWidth;
        private int _gridHeight;
        private IGridService _gridService;
        private float _gridLeft, _gridRight, _gridTop, _gridBottom;
        private int _lastScannedIndex = -1;
        private ConveyorEdge _lastEdge = ConveyorEdge.None;
        private float _pullDuration;

        public void Init(float pullDuration)
        {
            _gridService = ServiceLocator.Get<IGridService>();
            _cube = GetComponent<Cube>();
            _targetColor = (ProductColor)(int)_cube.Color;
            _pullDuration = pullDuration;

            var dims = _gridService.GetGridDimensions();
            _gridWidth = dims.x;
            _gridHeight = dims.y;
            _cellSize = _gridService.GetCellSize();
            _gridOrigin = _gridService.GetGridRoot().position;

            var offsetX = (_gridWidth - 1) * _cellSize / 2f;
            var offsetZ = (_gridHeight - 1) * _cellSize / 2f;

            _gridLeft   = _gridOrigin.x - offsetX;
            _gridRight  = _gridOrigin.x + offsetX;
            _gridTop    = _gridOrigin.z + offsetZ;
            _gridBottom = _gridOrigin.z - offsetZ;

            ScanLoop().Forget();
        }

        private async UniTaskVoid ScanLoop()
        {
            var token = this.GetCancellationTokenOnDestroy();

            while (!token.IsCancellationRequested)
            {
                var pos = transform.position;
                var edge = DetectEdge(pos);

                if (edge != ConveyorEdge.None)
                {
                    var index = GetScanIndex(pos, edge);

                    if (edge != _lastEdge)
                    {
                        _lastEdge = edge;
                        _lastScannedIndex = -1;
                    }

                    if (index != _lastScannedIndex && index >= 0)
                    {
                        _lastScannedIndex = index;
                        ScanForEdge(edge, index);
                    }
                }

                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }
        }

        private ConveyorEdge DetectEdge(Vector3 pos)
        {
            var margin = _cellSize * 0.5f;

            var nearBottom = pos.z < _gridBottom - margin;
            var nearTop    = pos.z > _gridTop + margin;
            var nearLeft   = pos.x < _gridLeft - margin;
            var nearRight  = pos.x > _gridRight + margin;

            if (nearBottom) return ConveyorEdge.Bottom;
            if (nearLeft)   return ConveyorEdge.Left;
            if (nearTop)    return ConveyorEdge.Top;
            if (nearRight)  return ConveyorEdge.Right;

            return ConveyorEdge.None;
        }

        private int GetScanIndex(Vector3 pos, ConveyorEdge edge)
        {
            switch (edge)
            {
                case ConveyorEdge.Bottom:
                case ConveyorEdge.Top:
                {
                    float localX = pos.x - _gridOrigin.x + (_gridWidth - 1) * _cellSize / 2f;
                    int col = Mathf.RoundToInt(localX / _cellSize);
                    return Mathf.Clamp(col, 0, _gridWidth - 1);
                }
                case ConveyorEdge.Left:
                case ConveyorEdge.Right:
                {
                    float localZ = -(pos.z - _gridOrigin.z - (_gridHeight - 1) * _cellSize / 2f);
                    int row = Mathf.RoundToInt(localZ / _cellSize);
                    return Mathf.Clamp(row, 0, _gridHeight - 1);
                }
            }

            return -1;
        }

        private void ScanForEdge(ConveyorEdge edge, int index)
        {
            Product product = null;

            switch (edge)
            {
                case ConveyorEdge.Bottom:
                    product = _gridService.FindAndRemoveMatchingProduct(index, _targetColor, true);
                    break;
                case ConveyorEdge.Top:
                    product = _gridService.FindAndRemoveMatchingProduct(index, _targetColor, false);
                    break;
                case ConveyorEdge.Left:
                    product = _gridService.FindAndRemoveMatchingProductByRow(index, _targetColor, false);
                    break;
                case ConveyorEdge.Right:
                    product = _gridService.FindAndRemoveMatchingProductByRow(index, _targetColor, true);
                    break;
            }

            if (product)
                PullProduct(product);
        }

        private void PullProduct(Product product)
        {
            product.transform.SetParent(null);

            var seq = DOTween.Sequence();
            seq.Append(product.transform.DOMove(transform.position, _pullDuration).SetEase(Ease.InBack));
            seq.Join(product.transform.DOScale(Vector3.zero, _pullDuration).SetEase(Ease.InBack));
            seq.OnComplete(() =>
            {
                Destroy(product.gameObject);
            });
        }
    }
}
