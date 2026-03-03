using _Game.Scripts.Audio;
using _Game.Scripts.Core.Slots;
using _Game.Scripts.Data;

namespace _Game.Scripts.Core.Cubes
{
    public interface ICubeProvider
    {
        public void Init(LevelData levelData, ISlotProvider slotProvider, IAudioService audioService);
        public void ClearCubes();
        public void StopAllConveyorCubes();
        public void StopAllBreaths();
    }
}
