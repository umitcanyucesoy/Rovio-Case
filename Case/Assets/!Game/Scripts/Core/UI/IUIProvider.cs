namespace _Game.Scripts.Core.UI
{
    public interface IUIProvider
    {
        void ShowWinPanel();
        void HideWinPanel();
        void ShowLosePanel();
        void HideLosePanel();
        void SetLevelText(int levelNumber);
        void SetCapacityText(int current, int max);
    }
}

