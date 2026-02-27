using _Game.Scripts.Data;

namespace _Game.Scripts.Core.Cubes
{
    public interface ICubeProvider
    {
        public void InitCubes(LevelData levelData);
        public void ClearCubes();
    }
}
