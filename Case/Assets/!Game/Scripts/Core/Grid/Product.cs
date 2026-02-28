using _Game.Scripts.Enums;
using UnityEngine;

namespace _Game.Scripts.Core.Grid
{
    public class Product : MonoBehaviour
    {
        [SerializeField] private new Renderer renderer;
        
        public ProductColor Color { get; private set; }

        public void SetColor(ProductColor color, Material material)
        {
            Color = color;
            renderer.sharedMaterial = material;
        }
    }
}