using System.Collections.Generic;
using _Game.Scripts.Core.Cubes;
using _Game.Scripts.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.Data
{
    [CreateAssetMenu(fileName = "CubeVisualData", menuName = "Game/Cube Visual Data")]
    public class CubeVisualData : ScriptableObject
    {
        [Title("Prefab")]
        public Cube cubePrefab;

        [Title("Materials")]
        [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "name")]
        public List<Material> colorMaterials = new();
        
        [Title("Path Follow")]
        public float followSpeed;
        [Range(0f, 1f)]
        public float startPercent = 0f;
        public Vector2 motionOffset = new Vector2(0f, 1f);

        [Title("Destroy Animation")] 
        public float moveYDuration;
        public float rotateDuration;
        public float scaleDuration;
        
        [Title("Jump Animation")]
        public float jumpPower;
        public float jumpDuration;
        public int jumpCount;
        
        [Title("Squash Animation")]
        public Vector3 punchScale;
        public float punchDuration;
        public int punchVibrato;
        public float punchElasticity;
        
        [Title("Punch Rotation")]
        public Vector3 punchRotation;
        public float punchRotDuration;
        public int punchRotVibrato;
        public float punchRotElasticity;
        
        [Title("Product Pull")]
        public float pullDuration = 0.3f;

        [Title("Column Shift")]
        public float shiftDuration = 0.25f;

        public Material GetMaterial(CubeColor color)
        {
            if (color == CubeColor.None || colorMaterials == null) return null;

            var index = (int)color - 1;
            if (index >= 0 && index < colorMaterials.Count)
                return colorMaterials[index];

            return null;
        }
    }
}
