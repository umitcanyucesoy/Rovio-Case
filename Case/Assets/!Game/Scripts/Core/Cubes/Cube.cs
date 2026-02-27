using UnityEngine;

namespace _Game.Scripts.Core.Cubes
{
    public class Cube : MonoBehaviour
    {
        [SerializeField] private new Renderer renderer;

        public void SetMaterial(Material material)
        {
            renderer.sharedMaterial = material;
        }
    }
}