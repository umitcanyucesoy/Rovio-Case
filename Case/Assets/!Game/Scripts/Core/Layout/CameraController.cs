using _Game.Scripts.Events;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.Core.Layout
{
    public class CameraController : MonoBehaviour
    {
        [Title("Scene References")]
        [SerializeField] private Transform topSection;
        [SerializeField] private Transform bottomSection;
        [SerializeField] private Camera mainCamera;

        [Title("Base Settings")]
        [SerializeField] private int baseSize = 10;
        [SerializeField] private float topZStep = -0.4f;
        [SerializeField] private float bottomZStep = -0.6f;
        [SerializeField] private float sizeStep = 0.65f;

        private Vector3 _topInitialPos;
        private Vector3 _bottomInitialPos;
        private float _baseCameraSize;

        public void Init()
        {
            _topInitialPos = topSection.position;
            _bottomInitialPos = bottomSection.position;
            _baseCameraSize = mainCamera.orthographicSize;

            EventBus.Subscribe<LevelLoadedEvent>(OnLevelLoaded);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<LevelLoadedEvent>(OnLevelLoaded);
        }

        private void OnLevelLoaded(LevelLoadedEvent e)
        {
            AdjustLayout(e.LevelData.width, e.LevelData.height);
        }

        private void AdjustLayout(int width, int height)
        {
            var heightExtra = Mathf.Max(0, height - baseSize);

            topSection.position = _topInitialPos + new Vector3(0f, 0f, heightExtra * topZStep);
            bottomSection.position = _bottomInitialPos + new Vector3(0f, 0f, heightExtra * bottomZStep);

            var widthExtra = Mathf.Max(0, width - baseSize);
            var maxExtra = Mathf.Max(widthExtra, heightExtra);
            mainCamera.orthographicSize = _baseCameraSize + maxExtra * sizeStep;
        }
    }
}
