using UnityEngine;

namespace _Game.Scripts.Core.Grid
{
    public class Product : MonoBehaviour
    {
        [SerializeField] private new Renderer renderer;

        public void SetColor(Color color)
        {
            renderer.material.color = color;
        }
    }
}