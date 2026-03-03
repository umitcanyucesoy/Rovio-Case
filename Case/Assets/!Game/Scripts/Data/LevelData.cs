
using System.Collections.Generic;
using _Game.Scripts.Core.Cubes;
using _Game.Scripts.Enums;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace _Game.Scripts.Data
{
    [CreateAssetMenu(fileName = "Level_", menuName = "Game/Level Data")]
    public class LevelData : SerializedScriptableObject
    {
        [OnValueChanged(nameof(UpdateGridSize))]
        public int width;
        
        [OnValueChanged(nameof(UpdateGridSize))]
        public int height;

        [Space(10)]
        [Title("Product Grid")]
        [TableMatrix(SquareCells = true, DrawElementMethod = "DrawCell")]
        public ProductColor[,] GridMatrix;

        [Space(10)]
        [Title("Cube Grid")]
        public List<ColumnData> cubeRows = new();

        private void UpdateGridSize()
        {
            GridMatrix = new ProductColor[width, height];
        }
        
#if UNITY_EDITOR
        [Space(10)]
        [Title("Color Balance")]
        [OnInspectorGUI(nameof(DrawColorBalance))]
        [PropertyOrder(100)]
        [SerializeField, HideInInspector] private bool balanceDummy;

        private void DrawColorBalance()
        {
            var gridCounts = new Dictionary<ProductColor, int>();
            var cubeTotals = new Dictionary<CubeColor, int>();

            if (GridMatrix != null)
            {
                foreach (var cell in GridMatrix)
                {
                    if (cell == ProductColor.Null) continue;
                    gridCounts.TryAdd(cell, 0);
                    gridCounts[cell]++;
                }
            }

            if (cubeRows != null)
            {
                foreach (var row in cubeRows)
                {
                    if (row?.columns == null) continue;
                    foreach (var cube in row.columns)
                    {
                        if (cube.color == CubeColor.None) continue;
                        cubeTotals.TryAdd(cube.color, 0);
                        cubeTotals[cube.color] += cube.value;
                    }
                }
            }

            var allColors = new[] { CubeColor.Red, CubeColor.Blue, CubeColor.Green, CubeColor.Yellow, CubeColor.Orange };

            var hasAny = false;
            foreach (var c in allColors)
            {
                var pc = (ProductColor)(int)c;
                var grid = gridCounts.GetValueOrDefault(pc, 0);
                var cube = cubeTotals.GetValueOrDefault(c, 0);
                if (grid > 0 || cube > 0) { hasAny = true; break; }
            }

            if (!hasAny)
            {
                EditorGUILayout.HelpBox("No colors assigned yet.", MessageType.Info);
                return;
            }

            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUILayout.Label("Color", EditorStyles.boldLabel, GUILayout.Width(70));
            GUILayout.Label("Grid", EditorStyles.boldLabel, GUILayout.Width(50));
            GUILayout.Label("Cubes", EditorStyles.boldLabel, GUILayout.Width(50));
            GUILayout.Label("Status", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();

            foreach (var c in allColors)
            {
                var pc = (ProductColor)(int)c;
                var grid = gridCounts.GetValueOrDefault(pc, 0);
                var cube = cubeTotals.GetValueOrDefault(c, 0);

                if (grid == 0 && cube == 0) continue;

                var guiColor = c switch
                {
                    CubeColor.Red => Color.red,
                    CubeColor.Blue => new Color(0.3f, 0.5f, 1f),
                    CubeColor.Green => Color.green,
                    CubeColor.Yellow => Color.yellow,
                    CubeColor.Orange => new Color(1f, 0.5f, 0f),
                    _ => Color.white
                };

                string status;
                Color statusColor;
                if (grid == cube) { status = "✓ OK"; statusColor = Color.green; }
                else if (cube > grid) { status = $"▲ too much +{cube - grid}"; statusColor = Color.yellow; }
                else { status = $"▼ Need -{grid - cube}"; statusColor = new Color(1f, 0.4f, 0.4f); }

                EditorGUILayout.BeginHorizontal();

                var prevBg = GUI.backgroundColor;
                GUI.backgroundColor = guiColor;
                GUILayout.Label(c.ToString(), new GUIStyle(EditorStyles.miniButton)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontStyle = FontStyle.Bold,
                    normal = { textColor = Color.white }
                }, GUILayout.Width(70));
                GUI.backgroundColor = prevBg;

                GUILayout.Label(grid.ToString(), GUILayout.Width(50));
                GUILayout.Label(cube.ToString(), GUILayout.Width(50));

                var prevColor = GUI.contentColor;
                GUI.contentColor = statusColor;
                GUILayout.Label(status, new GUIStyle(EditorStyles.boldLabel)
                {
                    normal = { textColor = statusColor }
                });
                GUI.contentColor = prevColor;

                EditorGUILayout.EndHorizontal();
            }
        }

        private ProductColor DrawCell(Rect rect, ProductColor value)
        {
            var e = Event.current;

            if (e.type == EventType.KeyDown && rect.Contains(e.mousePosition))
            {
                var newValue = e.keyCode switch
                {
                    KeyCode.Alpha0 or KeyCode.Keypad0 => ProductColor.Null,
                    KeyCode.Alpha1 or KeyCode.Keypad1 => ProductColor.Red,
                    KeyCode.Alpha2 or KeyCode.Keypad2 => ProductColor.Blue,
                    KeyCode.Alpha3 or KeyCode.Keypad3 => ProductColor.Green,
                    KeyCode.Alpha4 or KeyCode.Keypad4 => ProductColor.Yellow,
                    KeyCode.Alpha5 or KeyCode.Keypad5 => ProductColor.Orange,
                    _ => (ProductColor?)null
                };

                if (newValue.HasValue)
                {
                    value = newValue.Value;
                    GUI.changed = true;
                    e.Use();
                }
            }

            var color = value switch
            {
                ProductColor.Red => Color.red,
                ProductColor.Blue => Color.blue,
                ProductColor.Green => Color.green,
                ProductColor.Yellow => Color.yellow,
                ProductColor.Orange => new Color(1f, 0.5f, 0f),
                ProductColor.Null => Color.gray,
                _ => Color.white
            };

            EditorGUI.DrawRect(rect, color);

            var label = value == ProductColor.Null ? "·" : value.ToString();
            EditorGUI.LabelField(rect, label,
                new GUIStyle
                {
                    alignment = TextAnchor.MiddleCenter,
                    normal = { textColor = Color.black },
                    fontStyle = FontStyle.Bold
                });

            return value;
        }
#endif
    }
}
