using _Game.Scripts.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.Core.Cubes
{
    public class Cube : MonoBehaviour
    {
        [Title("References")] 
        [SerializeField] private new Renderer renderer;

        public void SetColor(CubeColor cubeColor)
        {
            renderer.material.color = cubeColor switch
            {
                CubeColor.Red    => Color.red,
                CubeColor.Blue   => Color.blue,
                CubeColor.Green  => Color.green,
                CubeColor.Yellow => Color.yellow,
                CubeColor.Orange => new Color(1f, 0.5f, 0f),
                _                => Color.white
            };
        }
    }
}