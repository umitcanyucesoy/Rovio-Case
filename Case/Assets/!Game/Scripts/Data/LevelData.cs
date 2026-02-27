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
        public bool[,] GridMatrix;

        private void UpdateGridSize()
        {
            GridMatrix = new bool[width, height];
        }
        
#if UNITY_EDITOR
        private bool DrawCell(Rect rect, bool value)
        {
            return UnityEditor.EditorGUI.Toggle(rect, value);
        }
#endif
    }
}