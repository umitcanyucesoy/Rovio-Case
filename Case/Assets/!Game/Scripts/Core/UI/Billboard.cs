using UnityEngine;

namespace _Game.Scripts.Core.UI
{
    public class Billboard : MonoBehaviour
    {
        private Camera _camera;
        private void Start() => _camera = Camera.main;
        private void LateUpdate() => transform.rotation = _camera.transform.rotation;
    }
}