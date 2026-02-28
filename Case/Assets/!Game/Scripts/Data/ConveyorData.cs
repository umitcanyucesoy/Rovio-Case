using UnityEngine;

namespace _Game.Scripts.Data
{
    [CreateAssetMenu(fileName = "NewConveyorData", menuName = "Game/ConveyorData")]
    public class ConveyorData : ScriptableObject
    {
        [Header("Spline Shape")]
        public float paddingCells = 0.75f;     
        public float cornerRadiusCells = 0.75f; 
        public int cornerSubdivisions = 6;    
        public float endGapCells = 0.5f; 
        public float yHeight = 0.0f;
        public float straightStepCells = 1.0f;

        [Header("Follower")]
        public float followSpeed = 3f;
        [Range(0f, 1f)]
        public float startPercent = 0f;
        public Vector2 motionOffset = new Vector2(0f, 1f);

        [Header("Jump Animation")]
        public float jumpPower = 1.5f;
        public float jumpDuration = 0.5f;
        public int jumpCount = 1;

        [Header("Squash Animation")]
        public Vector3 punchScale = new Vector3(0.15f, -0.3f, 0.15f);
        public float punchDuration = 0.35f;
        public int punchVibrato = 8;
        [Range(0f, 1f)]
        public float punchElasticity = 0.6f;

        [Header("Product Pull")]
        public float pullDuration = 0.3f;

        [Header("Column Shift")]
        public float shiftDuration = 0.25f;

        [Header("Slot Animation")]
        public float slotJumpPower = 1.5f;
        public float slotJumpDuration = 0.5f;
        public float slotRotateDuration = 0.3f;
    }
}