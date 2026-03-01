using System.Collections.Generic;
using _Game.Scripts.Core.Cubes;
using _Game.Scripts.Data;
using _Game.Scripts.Enums;
using Cysharp.Threading.Tasks;
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
        [SerializeField] private CubeVisualData cubeData;

        private readonly Cube[] _slotOccupants = new Cube[5];

        public bool TryPlaceInSlot(Cube cube)
        {
            var index = GetFirstEmptySlot();
            if (index < 0) return false;

            _slotOccupants[index] = cube;
            cube.SetState(CubeState.MovingToSlot);

            PlaceInSlotAsync(cube, index).Forget();
            return true;
        }

        private async UniTaskVoid PlaceInSlotAsync(Cube cube, int index)
        {
            var targetPos = slotTransforms[index].position + new Vector3(0f, .7f, 0f);
            var targetRot = slotTransforms[index].rotation;

            var jumpTween = cube.transform
                .DOJump(targetPos, cubeData.jumpPower, cubeData.jumpCount, .35f)
                .SetSpeedBased()
                .SetEase(Ease.InOutQuad);

            cube.transform.DORotateQuaternion(targetRot, jumpTween.Duration())
                .SetEase(Ease.InOutQuad);

            await jumpTween.AsyncWaitForCompletion();

            cube.SetState(CubeState.InSlot);
            cube.SetOutline(true);

            cube.Visual.DOPunchScale(cubeData.punchScale, cubeData.punchDuration, cubeData.punchVibrato, cubeData.punchElasticity);
            cube.Visual.DOPunchRotation(cubeData.punchRotation, cubeData.punchRotDuration, cubeData.punchRotVibrato, cubeData.punchRotElasticity);
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

        public void ClearSlots()
        {
            for (int i = 0; i < _slotOccupants.Length; i++)
                _slotOccupants[i] = null;
        }
    }
}
