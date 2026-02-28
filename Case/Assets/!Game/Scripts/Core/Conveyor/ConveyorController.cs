using System;
using System.Collections.Generic;
using _Game.Scripts.Core.Grid;
using _Game.Scripts.Data;
using _Game.Scripts.Events;
using Sirenix.OdinInspector;
using UnityEngine;
using Dreamteck.Splines;

namespace _Game.Scripts.Core.Conveyor
{
    public class ConveyorController : MonoBehaviour
    {
        [Title("References")]
        [SerializeField] private SplineComputer spline;
        [SerializeField] private GridData gridData;
        [SerializeField] private ConveyorData conveyorData;

        private void OnEnable() => EventBus.Subscribe<LevelLoadedEvent>(OnLevelLoaded);
        private void OnDisable() => EventBus.Unsubscribe<LevelLoadedEvent>(OnLevelLoaded);
        private void OnLevelLoaded(LevelLoadedEvent handle) => Build(handle.LevelData);
        
        private void Build(LevelData levelData)
        {
            int gridWidth = levelData.GridMatrix.GetLength(0);
            int gridHeight = levelData.GridMatrix.GetLength(1);

            if (gridWidth <= 0 || gridHeight <= 0)
            {
                Debug.LogError($"[ConveyorController] Invalid grid size: {gridWidth}x{gridHeight}");
                return;
            }

            float cellSize = gridData.cellSize;
            Transform gridRoot = gridData.gridContainer;

            float offsetX = (gridWidth - 1) * cellSize / 2f;
            float offsetZ = (gridHeight - 1) * cellSize / 2f;

            float minX = -offsetX;
            float maxX = (gridWidth - 1) * cellSize - offsetX;

            float maxZ = offsetZ;
            float minZ = -(gridHeight - 1) * cellSize + offsetZ;

            float p = conveyorData.paddingCells * cellSize;

            Vector3 p0 = gridRoot.position + new Vector3(minX - p, conveyorData.yHeight, maxZ + p);
            Vector3 p1 = gridRoot.position + new Vector3(maxX + p, conveyorData.yHeight, maxZ + p);
            Vector3 p2 = gridRoot.position + new Vector3(maxX + p, conveyorData.yHeight, minZ - p);
            Vector3 p3 = gridRoot.position + new Vector3(minX - p, conveyorData.yHeight, minZ - p);
            

            var points = BuildOpenRoundedPerimeterPoints(
                gridRoot, gridWidth, gridHeight, cellSize,
                conveyorData.paddingCells, conveyorData.cornerRadiusCells, conveyorData.cornerSubdivisions,
                conveyorData.endGapCells, conveyorData.yHeight, conveyorData.straightStepCells
            );

            spline.SetPoints(points);
            spline.Rebuild();
        }

        private SplinePoint[] BuildOpenRoundedPerimeterPoints(
            Transform gridRoot,
            int gridWidth,
            int gridHeight,
            float cellSize,
            float paddingCells,
            float cornerRadiusCells,
            int cornerSubdivisions,
            float endGapCells,
            float yHeight,
            float straightStepCells
        )
        {
            float offsetX = (gridWidth - 1) * cellSize / 2f;
            float offsetZ = (gridHeight - 1) * cellSize / 2f;

            float minX = -offsetX;
            float maxX = (gridWidth - 1) * cellSize - offsetX;

            float maxZ = offsetZ;
            float minZ = -(gridHeight - 1) * cellSize + offsetZ;

            float p = paddingCells * cellSize;
            float r = Mathf.Max(0.0001f, cornerRadiusCells * cellSize);
            int sub = Mathf.Max(1, cornerSubdivisions);
            float endGap = Mathf.Max(0.0f, endGapCells * cellSize);
            float step = Mathf.Max(0.0001f, straightStepCells * cellSize);

            float left = minX - p;
            float right = maxX + p;
            float top = maxZ + p;
            float bottom = minZ - p;

            float leftIn = left + r;
            float rightIn = right - r;
            float topIn = top - r;
            float bottomIn = bottom + r;

            var pts = new List<Vector3>(512);
            Vector3 root = gridRoot.position;

            void Add(Vector3 w)
            {
                if (pts.Count == 0 || (pts[pts.Count - 1] - w).sqrMagnitude > 0.0000005f)
                    pts.Add(w);
            }

            void AddLine(Vector3 from, Vector3 to)
            {
                float dist = Vector3.Distance(from, to);
                int count = Mathf.Max(1, Mathf.CeilToInt(dist / step));
                for (int i = 0; i <= count; i++)
                {
                    float t = (float)i / count;
                    Add(Vector3.Lerp(from, to, t));
                }
            }

            void AddArc(Vector3 center, float startDeg, float endDeg)
            {
                for (int i = 0; i <= sub; i++)
                {
                    float t = (float)i / sub;
                    float a = Mathf.Deg2Rad * Mathf.Lerp(startDeg, endDeg, t);
                    float x = center.x + Mathf.Cos(a) * r;
                    float z = center.z + Mathf.Sin(a) * r;
                    Add(new Vector3(x, yHeight, z));
                }
            }

            Vector3 bottomStart = root + new Vector3(rightIn, yHeight, bottom);
            Vector3 bottomEnd   = root + new Vector3(leftIn,  yHeight, bottom);
            AddLine(bottomStart, bottomEnd);

            AddArc(root + new Vector3(leftIn, yHeight, bottomIn), -90f, -180f);
            AddLine(root + new Vector3(left, yHeight, bottomIn),
                    root + new Vector3(left, yHeight, topIn));

            AddArc(root + new Vector3(leftIn, yHeight, topIn), 180f, 90f);
            AddLine(root + new Vector3(leftIn, yHeight, top),
                    root + new Vector3(rightIn, yHeight, top));

            AddArc(root + new Vector3(rightIn, yHeight, topIn), 90f, 0f);

            float endZ = bottomIn + endGap;
            AddLine(root + new Vector3(right, yHeight, topIn),
                    root + new Vector3(right, yHeight, endZ));

            var sp = new SplinePoint[pts.Count];
            for (int i = 0; i < pts.Count; i++) sp[i] = new SplinePoint(pts[i]);
            return sp;
        }
    }
}