using UnityEngine;

namespace _Game.Scripts.Data
{
    [CreateAssetMenu(fileName = "NewGridData", menuName = "Game/Grid Data")]
    public class GridData : ScriptableObject
    {
        public Transform gridContainer;
        public float cellSize = 1f;
    }
}