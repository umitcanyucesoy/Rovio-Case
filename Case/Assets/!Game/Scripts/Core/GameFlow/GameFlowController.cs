using _Game.Scripts.Events;
using UnityEngine;

namespace _Game.Scripts.Core.GameFlow
{
    public class GameFlowController : MonoBehaviour
    {
        private void OnEnable()
        {
            EventBus.Subscribe<GameWinEvent>(OnGameWin);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<GameWinEvent>(OnGameWin);
        }

        private void OnGameWin(GameWinEvent e)
        {
            Debug.Log("[GameFlowController] WIN!");
        }
    }
}
