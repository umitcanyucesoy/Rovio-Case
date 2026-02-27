using _Game.Scripts.Enums;
using Sirenix.OdinInspector;
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
        [TableMatrix(SquareCells = true, DrawElementMethod = nameof(DrawCell))]
        public ProductColor[,] GridMatrix;

        private void UpdateGridSize()
        {
            GridMatrix = new ProductColor[width, height];
        }
        
#if UNITY_EDITOR
        private ProductColor DrawCell(Rect rect, ProductColor value)
        {
            if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
            {
                value = (ProductColor)(((int)value + 1) % System.Enum.GetValues(typeof(ProductColor)).Length);
                GUI.changed = true;
                Event.current.Use();
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

            UnityEditor.EditorGUI.DrawRect(rect, color);
            UnityEditor.EditorGUI.LabelField(rect, value.ToString(), new GUIStyle { alignment = TextAnchor.MiddleCenter, normal = { textColor = Color.white } });

            return value;
        }
#endif
    }
}