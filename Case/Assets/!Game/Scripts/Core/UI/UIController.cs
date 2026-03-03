using DG.Tweening;
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
        [SerializeField] private TextMeshProUGUI capacityText;

        [Title("Capacity Warning")]
        [SerializeField] private Color capacityWarningColor = Color.red;
        [SerializeField] private float capacityPulseDuration = 0.4f;

        private Color _capacityOriginalColor;
        private Tween _capacityPulseTween;

        private void Awake() => _capacityOriginalColor = capacityText.color;

        public void ShowWinPanel() => winPanel.SetActive(true);
        public void HideWinPanel() => winPanel.SetActive(false);

        public void ShowLosePanel() => losePanel.SetActive(true);
        public void HideLosePanel() => losePanel.SetActive(false);

        public void SetLevelText(int levelNumber) => levelText.text = $"Level {levelNumber}";

        public void SetCapacityText(int current, int max)
        {
            capacityText.text = $"Capacity: {current}/{max}";

            if (current >= max)
                StartCapacityWarning();
            else
                StopCapacityWarning();
        }

        private void StartCapacityWarning()
        {
            if (_capacityPulseTween is { active: true }) return;

            _capacityPulseTween = capacityText
                .DOColor(capacityWarningColor, capacityPulseDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }

        private void StopCapacityWarning()
        {
            if (_capacityPulseTween != null)
            {
                _capacityPulseTween.Kill();
                _capacityPulseTween = null;
            }

            if (capacityText)
                capacityText.color = _capacityOriginalColor;
        }
    }
}
