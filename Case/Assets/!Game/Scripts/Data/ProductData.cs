using System.Collections.Generic;
using _Game.Scripts.Core.Grid;
using _Game.Scripts.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.Data
{
    [CreateAssetMenu(fileName = "ProductData", menuName = "Game/Product Data")]
    public class ProductData : ScriptableObject
    {
        public Product productPrefab;
        
        [Title("Materials")]
        [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "name")]
        public List<Material> colorMaterials = new();

        public Material GetMaterial(ProductColor color)
        {
            if (color == ProductColor.Null || colorMaterials == null)
                return null;
            
            var index = (int)color;
            if (index >= 0 && index < colorMaterials.Count)
                return colorMaterials[index];
            
            return null;
        }
    }
}