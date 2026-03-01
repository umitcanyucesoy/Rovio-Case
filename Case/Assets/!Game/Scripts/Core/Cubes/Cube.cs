using _Game.Scripts.Enums;
using UnityEngine;

namespace _Game.Scripts.Core.Cubes
{
    public class Cube : MonoBehaviour
    {
        [SerializeField] private new Renderer renderer;
        
        public Transform Visual => renderer.transform;
        public CubeColor Color { get; private set; }
        public CubeState State { get; private set; } = CubeState.InQueue;

        public void SetColor(CubeColor color, Material material)
        {
            Color = color;
            renderer.sharedMaterial = material;
        }

        public void SetState(CubeState state)
        {
            State = state;
        }
    }
}