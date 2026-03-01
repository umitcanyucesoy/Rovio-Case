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
        IInputService _inputService;
        private IUIProvider _uiProvider;
        private ILevelProvider _levelProvider;

        public void Init(IUIProvider uiProvider, ILevelProvider levelProvider)
        {
            _uiProvider = uiProvider;
            _levelProvider = levelProvider;
            _inputService = ServiceLocator.Get<IInputService>();
            
            EventBus.Subscribe<GameWinEvent>(OnGameWin);
            EventBus.Subscribe<GameLoseEvent>(OnGameLose);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<GameWinEvent>(OnGameWin);
            EventBus.Unsubscribe<GameLoseEvent>(OnGameLose);
        }

        private void OnGameWin(GameWinEvent e)
        {
            Debug.Log("[GameFlowController] WIN!");
            _inputService.SetEnabled(false);
            _uiProvider.ShowWinPanel();
        }

        private void OnGameLose(GameLoseEvent e)
        {
            Debug.Log("[GameFlowController] LOSE!");
            _inputService.SetEnabled(false);
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
