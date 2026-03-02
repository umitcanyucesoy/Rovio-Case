using _Game.Scripts.Audio;
using _Game.Scripts.Core.Cubes;
using _Game.Scripts.Core.Input;
using _Game.Scripts.Core.Levels;
using _Game.Scripts.Core.UI;
using _Game.Scripts.Events;
using _Game.Scripts.Services;
using UnityEngine;

namespace _Game.Scripts.Core.GameFlow
{
    public class GameFlowController : MonoBehaviour
    {
        private IInputService _inputService;
        private IAudioService _audioService;
        private IUIProvider _uiProvider;
        private ILevelProvider _levelProvider;
        private ICubeProvider _cubeProvider;

        public void Init(IUIProvider uiProvider, ILevelProvider levelProvider, ICubeProvider cubeProvider)
        {
            _uiProvider = uiProvider;
            _levelProvider = levelProvider;
            _cubeProvider = cubeProvider;
            _inputService = ServiceLocator.Get<IInputService>();
            _audioService = ServiceLocator.Get<IAudioService>();
            
            EventBus.Subscribe<GameWinEvent>(OnGameWin);
            EventBus.Subscribe<GameLoseEvent>(OnGameLose);
            EventBus.Subscribe<LevelLoadedEvent>(OnLevelLoaded);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<GameWinEvent>(OnGameWin);
            EventBus.Unsubscribe<GameLoseEvent>(OnGameLose);
            EventBus.Unsubscribe<LevelLoadedEvent>(OnLevelLoaded);
        }

        private void OnLevelLoaded(LevelLoadedEvent e) => _uiProvider.SetLevelText(e.LevelNumber);
        
        private void OnGameWin(GameWinEvent e)
        {
            Debug.Log("[GameFlowController] WIN!");
            _inputService.SetEnabled(false);
            _cubeProvider.StopAllConveyorCubes();
            _audioService.Play("Win");
            _uiProvider.ShowWinPanel();
        }

        private void OnGameLose(GameLoseEvent e)
        {
            Debug.Log("[GameFlowController] LOSE!");
            _inputService.SetEnabled(false);
            _cubeProvider.StopAllConveyorCubes();
            _audioService.Play("Lose");
            _uiProvider.ShowLosePanel();
        }

        public void OnNextLevelButtonClicked()
        {
            _uiProvider.HideWinPanel();
            _inputService.SetEnabled(true);
            _levelProvider.NextLevel();
        }

        public void OnRetryButtonClicked()
        {
            _uiProvider.HideLosePanel();
            _inputService.SetEnabled(true);
            _levelProvider.LoadLevel();
        }
    }
}
