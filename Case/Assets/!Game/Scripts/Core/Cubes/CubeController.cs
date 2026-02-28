using System;
using System.Collections.Generic;
using _Game.Scripts.Core.Conveyor;
using _Game.Scripts.Data;
using _Game.Scripts.Enums;
using _Game.Scripts.Events;
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
        [SerializeField] private CubeVisualData cubeVisualData;
        [SerializeField] private Transform cubeContainer;
        [SerializeField] private float spacing = 1.1f;

        [Title("Conveyor")]
        [SerializeField] private SplineComputer spline;
        [SerializeField] private ConveyorData conveyorData;

        private readonly List<List<Cube>> _spawnedCubes = new();

        private void OnDisable()
        {
            EventBus.Unsubscribe<CubeClickedEvent>(OnCubeClicked);
        }

        private void OnCubeClicked(CubeClickedEvent handle) => PlaceCubeOnConveyor(handle.Cube);

        public void InitCubes(LevelData levelData)
        {
            ClearCubes();
            GenerateCubes(levelData.cubeRows);
            EventBus.Subscribe<CubeClickedEvent>(OnCubeClicked);
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
                    var position = new Vector3((rowIndex * spacing) - offset, 0, -colIndex * spacing);

                    var cube = Instantiate(cubeVisualData.cubePrefab, cubeContainer.position + position, Quaternion.identity, cubeContainer);
                    var material = cubeVisualData.GetMaterial(cubeData.color);
                    cube.SetColor(cubeData.color, material);
                    cube.name = $"Cube_R{rowIndex}_C{colIndex} (Value: {cubeData.value})";

                    row.Add(cube);
                }

                _spawnedCubes.Add(row);
            }
        }

        private void PlaceCubeOnConveyor(Cube cube)
        {
            cube.transform.SetParent(null);

            var startPos = spline.EvaluatePosition(conveyorData.startPercent);
            var targetPos = startPos + new Vector3(0f, conveyorData.motionOffset.y, 0f);

            var seq = DOTween.Sequence();

            seq.Append(cube.transform.DOJump(targetPos, conveyorData.jumpPower, conveyorData.jumpCount, conveyorData.jumpDuration)
                .SetEase(Ease.InOutQuad));

            seq.AppendCallback(() =>
            {
                var follower = cube.gameObject.AddComponent<SplineFollower>();
                follower.spline = spline;
                follower.followMode = SplineFollower.FollowMode.Uniform;
                follower.followSpeed = conveyorData.followSpeed;
                follower.motion.rotationOffset = new Vector3(0f, 90f, 0f);
                follower.motion.offset = conveyorData.motionOffset;
                follower.SetPercent(conveyorData.startPercent);
                follower.follow = true;

                if (cube.TryGetComponent(out CubeProductScanner scanner))
                    scanner.Init(conveyorData.pullDuration);

                follower.onEndReached += _ =>
                {
                    follower.follow = false;
                };
            });

            seq.Append(cube.transform.DOPunchScale(
                conveyorData.punchScale,
                conveyorData.punchDuration,
                conveyorData.punchVibrato,
                conveyorData.punchElasticity));
        }

        public void ClearCubes()
        {
            _spawnedCubes.Clear();
            
            if (!cubeContainer) return;
            for (int i = cubeContainer.childCount - 1; i >= 0; i--)
                DestroyImmediate(cubeContainer.GetChild(i).gameObject);
        }
    }
}

