using System.Collections.Generic;
using _Game.Scripts.Core.Cubes;
using _Game.Scripts.Data;
using _Game.Scripts.Enums;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.Core.Slots
{
    public class SlotController : MonoBehaviour, ISlotProvider
    {
        [Title("Slots")]
        [SerializeField] private List<Transform> slotTransforms = new();

        [Title("Settings")]
        [SerializeField] private ConveyorData conveyorData;

        private readonly Cube[] _slotOccupants = new Cube[5];

        public bool TryPlaceInSlot(Cube cube)
        {
            var index = GetFirstEmptySlot();
            if (index < 0) return false;

            _slotOccupants[index] = cube;
            cube.SetState(CubeState.InSlot);

            var targetPos = slotTransforms[index].position;
            var targetRot = slotTransforms[index].rotation;

            var seq = DOTween.Sequence();
            seq.Append(cube.transform.DOJump(targetPos, conveyorData.slotJumpPower, 1, conveyorData.slotJumpDuration)
                .SetEase(Ease.InOutQuad));
            seq.Join(cube.transform.DORotateQuaternion(targetRot, conveyorData.slotRotateDuration)
                .SetEase(Ease.InOutQuad));
            seq.Append(cube.transform.DOPunchScale(
                conveyorData.punchScale,
                conveyorData.punchDuration,
                conveyorData.punchVibrato,
                conveyorData.punchElasticity));

            return true;
        }

        public bool RemoveFromSlot(Cube cube)
        {
            for (int i = 0; i < _slotOccupants.Length; i++)
            {
                if (Equals(_slotOccupants[i], cube))
                {
                    _slotOccupants[i] = null;
                    return true;
                }
            }

            return false;
        }

        private int GetFirstEmptySlot()
        {
            for (int i = 0; i < _slotOccupants.Length; i++)
                if (!_slotOccupants[i]) return i;

            return -1;
        }
    }
}
