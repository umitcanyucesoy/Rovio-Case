using System.Collections.Generic;
using _Game.Scripts.Audio;
using _Game.Scripts.Core.Cubes;
using _Game.Scripts.Core.Grid;
using _Game.Scripts.Core.Slots;
using _Game.Scripts.Data;
using _Game.Scripts.Events;
using _Game.Scripts.Services;
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
        
        private IAudioService _audioService;
        private ICubeProvider _cubeProvider;
        private ISlotProvider _slotProvider;

        public void Init(ICubeProvider cubeProvider, ISlotProvider slotProvider, IAudioService audioService)
        {
            _cubeProvider = cubeProvider;
            _slotProvider = slotProvider;
            _audioService = audioService;
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

            _slotProvider.ClearSlots();
            ServiceLocator.Get<IGridService>().InitGrid(levelData);
            _cubeProvider.Init(levelData, _slotProvider, _audioService);
            
            EventBus.Publish(new LevelLoadedEvent(levelData, _currentLevelIndex + 1));
        }

        public void NextLevel()
        {
            _currentLevelIndex = (_currentLevelIndex + 1) % levels.Count;
            LoadLevel();
        }
    }
}