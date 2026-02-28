using UnityEngine;

namespace _Game.Scripts.Data
{
    [CreateAssetMenu(fileName = "NewConveyorData", menuName = "Game/ConveyorData")]
    public class ConveyorData : ScriptableObject
    {
        public float paddingCells = 0.75f;     
        public float cornerRadiusCells = 0.75f; 
        public int cornerSubdivisions = 6;    
        public float endGapCells = 0.5f; 
        public float yHeight = 0.0f;
        public float straightStepCells = 1.0f;
    }
}