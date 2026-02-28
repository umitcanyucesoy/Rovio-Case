using _Game.Scripts.Enums;
using UnityEngine;

namespace _Game.Scripts.Core.Cubes
{
    public class Cube : MonoBehaviour
    {
        [SerializeField] private new Renderer renderer;
        
        public CubeColor Color { get; private set; }

        public void SetColor(CubeColor color, Material material)
        {
            Color = color;
            renderer.sharedMaterial = material;
        }
    }
}