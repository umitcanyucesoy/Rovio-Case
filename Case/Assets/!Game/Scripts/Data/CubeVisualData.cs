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
        public Cube cubePrefab;

        [Title("Materials")]
        [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "name")]
        public List<Material> colorMaterials = new();

        public Material GetMaterial(CubeColor color)
        {
            if (colorMaterials == null) return null;

            var index = (int)color;
            if (index >= 0 && index < colorMaterials.Count)
                return colorMaterials[index];

            return null;
        }
    }
}
