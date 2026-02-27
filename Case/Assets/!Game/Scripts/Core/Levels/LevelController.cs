using System.Collections.Generic;
using _Game.Scripts.Core.Grid;
using _Game.Scripts.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.Core.Levels
{
    public class LevelController : MonoBehaviour, ILevelProvider
    {
        [Title("Level Settings")]
        [SerializeField] private List<LevelData> levels = new();

        [Title("Current Level")]
        [ReadOnly, ShowInInspector]
        private int _currentLevelIndex = 0;
        
        private IGridProvider _gridProvider;

        public void Init(IGridProvider gridProvider)
        {
            _gridProvider = gridProvider;
            LoadLevel();
        }

        public void LoadLevel()
        {
            if (_currentLevelIndex >= levels.Count)
            {
                Debug.LogError($"[LevelController] Invalid level index: {_currentLevelIndex}");
                return;
            }

            var levelData = levels[_currentLevelIndex];
            _gridProvider.InitGrid(levelData);
        }

        public void NextLevel()
        {
            if (_currentLevelIndex < levels.Count - 1)
            {
                _currentLevelIndex++;
                LoadLevel();
            }
            else
                Debug.Log("[LevelController] No more levels!");
            
        }
    }
}