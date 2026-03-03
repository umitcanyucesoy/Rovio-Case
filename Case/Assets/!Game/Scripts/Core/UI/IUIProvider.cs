namespace _Game.Scripts.Core.UI
{
    public interface IUIProvider
    {
        public void ShowWinPanel();
        public void HideWinPanel();
        public void ShowLosePanel();
        public void HideLosePanel();
        public void SetLevelText(int levelNumber);
        public void SetCapacityText(int current, int max);
    }
}

