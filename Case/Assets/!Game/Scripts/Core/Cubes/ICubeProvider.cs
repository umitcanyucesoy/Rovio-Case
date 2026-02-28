using _Game.Scripts.Core.Slots;
using _Game.Scripts.Data;

namespace _Game.Scripts.Core.Cubes
{
    public interface ICubeProvider
    {
        public void InitCubes(LevelData levelData, ISlotProvider slotProvider);
        public void ClearCubes();
    }
}
