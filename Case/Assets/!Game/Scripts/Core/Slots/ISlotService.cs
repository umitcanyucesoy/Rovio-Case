using _Game.Scripts.Core.Cubes;

namespace _Game.Scripts.Core.Slots
{
    public interface ISlotProvider
    {
        public bool TryPlaceInSlot(Cube cube);
        public bool RemoveFromSlot(Cube cube);
    }
}
