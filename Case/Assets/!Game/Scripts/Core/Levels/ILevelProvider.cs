using _Game.Scripts.Core.Cubes;
using _Game.Scripts.Core.Slots;

namespace _Game.Scripts.Core.Levels
{
    public interface ILevelProvider
    {
        public void Init(ICubeProvider cubeProvider, ISlotProvider slotProvider);
        public void LoadLevel();
        public void NextLevel();
    }
}