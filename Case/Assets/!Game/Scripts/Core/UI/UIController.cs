using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.Core.UI
{
    public class UIController : MonoBehaviour, IUIProvider
    {
        [Title("Panels")]
        [SerializeField] private GameObject winPanel;
        [SerializeField] private GameObject losePanel;

        [Title("Texts")]
        [SerializeField] private TextMeshProUGUI levelText;

        public void ShowWinPanel() => winPanel.SetActive(true);
        public void HideWinPanel() => winPanel.SetActive(false);

        public void ShowLosePanel() => losePanel.SetActive(true);
        public void HideLosePanel() => losePanel.SetActive(false);

        public void SetLevelText(int levelNumber) => levelText.text = $"Level {levelNumber}";
    }
}
