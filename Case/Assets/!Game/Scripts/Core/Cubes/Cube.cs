using _Game.Scripts.Audio;
using _Game.Scripts.Data;
using _Game.Scripts.Enums;
using _Game.Scripts.Events;
using _Game.Scripts.Services;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Dreamteck.Splines;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.Core.Cubes
{
    public class Cube : MonoBehaviour
    {
        [Title("References")]
        [SerializeField] private CubeVisualData cubeData;
        [SerializeField] private new Renderer renderer;
        [SerializeField] private TextMeshPro valueText;
        
        public Transform Visual => renderer.transform;
        public CubeColor Color { get; private set; }
        public CubeState State { get; private set; } = CubeState.InQueue;
        public int Value { get; private set; }

        private bool _isDestroying;
        private Vector3 _originalVisualScale;

        private void Start() => _originalVisualScale = Visual.localScale;

        public void SetColor(CubeColor color, Material material)
        {
            Color = color;
            renderer.sharedMaterial = material;
        }

        public void SetValue(int value)
        {
            Value = value;
            UpdateValueText();
        }

        public void SetState(CubeState state) => State = state;
        private void UpdateValueText() => valueText.text = Value.ToString();

        public void SetOutline(bool isActive)
        {
            var mats = renderer.sharedMaterials;

            if (isActive && mats.Length < 2 && cubeData.outlineMaterial)
            {
                renderer.sharedMaterials = new[] { mats[0], cubeData.outlineMaterial };
            }
            else if (!isActive && mats.Length > 1)
            {
                renderer.sharedMaterials = new[] { mats[0] };
            }
        }

        public void SetFade(float alpha, float duration)
        {
            valueText.DOKill();

            if (duration <= 0f)
            {
                var c = valueText.color;
                c.a = alpha;
                valueText.color = c;
            }
            else
                valueText.DOFade(alpha, duration);
        }

        public void ConsumePoint()
        {
            if (_isDestroying) return;

            Value--;
            UpdateValueText();

            Visual.DOKill();
            Visual.localScale = _originalVisualScale;
            Visual.DOPunchScale(cubeData.consumePunchScale, cubeData.consumePunchDuration, cubeData.consumePunchVibrato, cubeData.consumePunchElasticity);

            if (Value <= 0) DestroyCube().Forget();
        }

        private async UniTaskVoid DestroyCube()
        {
            _isDestroying = true;
            var previousState = State;
            State = CubeState.InQueue;

            if (TryGetComponent(out SplineFollower follower)) { follower.follow = false; }

            transform.DOKill();
            Visual.DOKill();

            var startPos = transform.position;
            var targetPos = startPos + new Vector3(0f, 2f, 0f);

            var moveTween = transform.DOMove(targetPos, cubeData.moveYDuration).SetEase(Ease.OutQuad);
            transform.DORotate(new Vector3(0f, 360f, 0f), cubeData.rotateDuration, RotateMode.FastBeyond360).SetEase(Ease.InQuad);
            transform.DOScale(Vector3.zero, cubeData.scaleDuration).SetEase(Ease.InQuad);

            await moveTween.AsyncWaitForCompletion();

            EventBus.Publish(new CubeDestroyedEvent(this, previousState));
            Destroy(gameObject);
        }

        public void InputFailEffect()
        {
            Visual.DOKill();
            Visual.localRotation = Quaternion.identity;
            Visual.DOPunchRotation(cubeData.denyShakeRotation, cubeData.denyShakeDuration, cubeData.denyShakeVibrato, cubeData.denyShakeElasticity);
            ServiceLocator.Get<IAudioService>().Play("InputFail");
        }

        public Color GetParticleColor()
        {
            return Color switch
            {
                CubeColor.Red => UnityEngine.Color.red,
                CubeColor.Blue => UnityEngine.Color.blue,
                CubeColor.Green => UnityEngine.Color.green,
                CubeColor.Yellow => UnityEngine.Color.yellow,
                CubeColor.Orange => new Color(1f, 0.5f, 0f),
                _ => UnityEngine.Color.white
            };
        }
    }
}