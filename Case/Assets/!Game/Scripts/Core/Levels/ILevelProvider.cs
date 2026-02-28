using _Game.Scripts.Core.Cubes;

namespace _Game.Scripts.Core.Levels
{
    public interface ILevelProvider
    {
        public void Init(ICubeProvider cubeProvider);
        public void LoadLevel();
        public void NextLevel();
    }
}