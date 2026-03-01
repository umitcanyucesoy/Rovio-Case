using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Core.Conveyor;
using _Game.Scripts.Core.Slots;
using _Game.Scripts.Data;
using _Game.Scripts.Enums;
using _Game.Scripts.Events;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Dreamteck.Splines;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.Core.Cubes
{
    [Serializable]
    public class CubeData
    {
        public CubeColor color;
        public int value;
    }

    [Serializable]
    public class ColumnData
    {
        [LabelText("Columns")]
        public List<CubeData> columns = new();
    }   
    
    public class CubeController : MonoBehaviour, ICubeProvider
    {
        [Title("Settings")]
        [SerializeField] private SplineComputer spline;
        [SerializeField] private CubeVisualData cubeVisualData;
        [SerializeField] private Transform cubeContainer;
        [SerializeField] private float spacing = 1.1f;

        private ISlotProvider _slotProvider;
        private readonly List<List<Cube>> _spawnedCubes = new();
        [ReadOnly, ShowInInspector] private readonly List<Cube> _allCubes = new();

        private void OnEnable()
        {
            EventBus.Subscribe<CubeClickedEvent>(OnCubeClicked);
            EventBus.Subscribe<CubeDestroyedEvent>(OnCubeDestroyed);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<CubeClickedEvent>(OnCubeClicked);
            EventBus.Unsubscribe<CubeDestroyedEvent>(OnCubeDestroyed);
        }

        public void InitCubes(LevelData levelData, ISlotProvider slotProvider)
        {
            _slotProvider = slotProvider;
            ClearCubes();
            GenerateCubes(levelData.cubeRows);
        }

        private void GenerateCubes(List<ColumnData> rows)
        {
            for (int rowIndex = 0; rowIndex < rows.Count; rowIndex++)
            {
                var row = new List<Cube>();
                var columnData = rows[rowIndex];

                float offset = (rows.Count - 1) * spacing / 2f;

                for (int colIndex = 0; colIndex < columnData.columns.Count; colIndex++)
                {
                    var cubeData = columnData.columns[colIndex];
                    
                    if (cubeData.color == CubeColor.None)
                    {
                        Debug.LogWarning($"[CubeController] Cube at R{rowIndex}_C{colIndex} has no color assigned!");
                        continue;
                    }
                    
                    var position = new Vector3((rowIndex * spacing) - offset, 0, -colIndex * spacing);

                    var cube = Instantiate(cubeVisualData.cubePrefab, cubeContainer.position + position, Quaternion.identity, cubeContainer);
                    var material = cubeVisualData.GetMaterial(cubeData.color);
                    cube.SetColor(cubeData.color, material);
                    cube.SetValue(cubeData.value);
                    cube.name = $"Cube_R{rowIndex}_C{colIndex} (Value: {cubeData.value})";

                    _allCubes.Add(cube);
                    row.Add(cube);
                }

                _spawnedCubes.Add(row);
            }
        }
        
        private void OnCubeClicked(CubeClickedEvent handle)
        {
            var cube = handle.Cube;

            switch (cube.State)
            {
                case CubeState.InQueue:
                    HandleQueueClick(cube);
                    break;
                case CubeState.InSlot:
                    HandleSlotClick(cube);
                    break;
                case CubeState.OnConveyor:
                    break;
            }
        }

        private void HandleQueueClick(Cube cube)
        {
            var foundRow = -1;
            
            for (int r = 0; r < _spawnedCubes.Count; r++)
            {
                var row = _spawnedCubes[r];
                if (row.Count > 0 && Equals(row[0], cube))
                {
                    foundRow = r;
                    break;
                }
            }

            if (foundRow < 0) return;

            _spawnedCubes[foundRow].RemoveAt(0);
            PlaceCubeOnConveyor(cube).Forget();
            ShiftColumnForward(foundRow);
        }

        private void HandleSlotClick(Cube cube)
        {
            _slotProvider.RemoveFromSlot(cube);
            PlaceCubeOnConveyor(cube).Forget();
        }

        private void ShiftColumnForward(int rowIndex)
        {
            var row = _spawnedCubes[rowIndex];
            if (row.Count == 0) return;

            var offset = (_spawnedCubes.Count - 1) * spacing / 2f;

            for (int i = 0; i < row.Count; i++)
            {
                var targetPos = cubeContainer.position + new Vector3((rowIndex * spacing) - offset, 0, -i * spacing);
                row[i].transform.DOMove(targetPos, cubeVisualData.shiftDuration).SetEase(Ease.OutQuad);
            }
        }

        private async UniTaskVoid PlaceCubeOnConveyor(Cube cube)
        {
            cube.transform.SetParent(null);

            var startPos = spline.EvaluatePosition(cubeVisualData.startPercent);
            var targetPos = startPos + new Vector3(0f, cubeVisualData.motionOffset.y, 0f);

            await cube.transform
                .DOJump(targetPos, cubeVisualData.jumpPower, cubeVisualData.jumpCount, cubeVisualData.jumpDuration)
                .SetSpeedBased()
                .SetEase(Ease.InOutQuad)
                .AsyncWaitForCompletion();

            cube.SetState(CubeState.OnConveyor);

            var follower = cube.gameObject.AddComponent<SplineFollower>();
            follower.spline = spline;
            follower.followMode = SplineFollower.FollowMode.Uniform;
            follower.followSpeed = cubeVisualData.followSpeed;
            follower.motion.rotationOffset = new Vector3(0f, 90f, 0f);
            follower.motion.offset = cubeVisualData.motionOffset;
            follower.SetPercent(cubeVisualData.startPercent);
            follower.follow = true;

            if (cube.TryGetComponent(out CubeProductScanner scanner))
                scanner.Init(cubeVisualData.pullDuration);

            follower.onEndReached += _ =>
            {
                follower.follow = false;
                Destroy(follower);
                
                if (!_slotProvider.TryPlaceInSlot(cube))
                {
                    EventBus.Publish(new GameLoseEvent());
                }
            };

            cube.transform.DOPunchScale(cubeVisualData.punchScale, cubeVisualData.punchDuration, cubeVisualData.punchVibrato, cubeVisualData.punchElasticity);
            cube.Visual.DOPunchRotation(cubeVisualData.punchRotation, cubeVisualData.punchRotDuration, cubeVisualData.punchRotVibrato, cubeVisualData.punchRotElasticity);
        }

        private void OnCubeDestroyed(CubeDestroyedEvent e)
        {
            _allCubes.Remove(e.Cube);
            if (_allCubes.Count <= 0) EventBus.Publish(new GameWinEvent());
        }

        public void ClearCubes()
        {
            foreach (var cube in _allCubes.Where(cube => cube))
            {
                cube.transform.DOKill();
                cube.Visual.DOKill();
                Destroy(cube.gameObject);
            }

            _spawnedCubes.Clear();
            _allCubes.Clear();
            
            if (!cubeContainer) return;
            for (int i = cubeContainer.childCount - 1; i >= 0; i--)
                DestroyImmediate(cubeContainer.GetChild(i).gameObject);
        }
    }
}
