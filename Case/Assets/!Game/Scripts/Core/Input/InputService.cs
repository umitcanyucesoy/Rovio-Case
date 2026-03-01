using _Game.Scripts.Core.Cubes;
using _Game.Scripts.Events;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Game.Scripts.Core.Input
{
    public class InputService : MonoBehaviour, IInputService
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private LayerMask cubeLayerMask;

        private bool _isEnabled = true;

        public void Init()
        {
            ListenForClicks().Forget();
        }

        public void SetEnabled(bool isEnabled)
        {
            _isEnabled = isEnabled;
        }

        private async UniTaskVoid ListenForClicks()
        {
            var token = this.GetCancellationTokenOnDestroy();

            while (!token.IsCancellationRequested)
            {
                await UniTask.WaitUntil(() => UnityEngine.Input.GetMouseButtonDown(0), cancellationToken: token);

                HandleClick(UnityEngine.Input.mousePosition);

                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }
        }

        private void HandleClick(Vector3 screenPosition)
        {
            if (!_isEnabled) return;
            
            var ray = mainCamera.ScreenPointToRay(screenPosition);

            if (!Physics.Raycast(ray, out var hit, Mathf.Infinity, cubeLayerMask)) return;

            var cube = hit.collider.GetComponentInParent<Cube>();
            if (cube == null) return;

            Debug.Log($"[InputService] Cube clicked: {cube.name}");
            EventBus.Publish(new CubeClickedEvent(cube));
        }
    }
}
