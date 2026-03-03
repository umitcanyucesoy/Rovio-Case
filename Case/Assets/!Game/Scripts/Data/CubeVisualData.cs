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
        public Material outlineMaterial;
        
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

        [Title("Queue Fade")]
        [Range(0f, 1f)]
        public float fadedAlpha = 0.35f;
        public float fadeDuration = 0.25f;

        [Title("Consume Punch")]
        public Vector3 consumePunchScale = new(0.15f, 0.15f, 0.15f);
        public float consumePunchDuration = 0.3f;
        public int consumePunchVibrato = 5;
        public float consumePunchElasticity = 0.5f;

        [Title("Deny Shake")]
        public Vector3 denyShakeRotation = new(0f, 15f, 0f);
        public float denyShakeDuration = 0.4f;
        public int denyShakeVibrato = 8;
        public float denyShakeElasticity = 0.3f;

        [Title("Breath Effect")]
        public float breathScale = 1.08f;
        public float breathDuration = 0.8f;
   
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
