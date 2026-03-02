using System;
using System.Collections.Generic;
using _Game.Scripts.Pool;
using _Game.Scripts.Services;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.Core.VFX
{
    [Serializable]
    public class ParticleEntry
    {
        public string name;
        public ParticleSystem prefab;
        [Min(1)] public int warmupCount = 3;
    }

    public class ParticleService : MonoBehaviour, IParticleService
    {
        [Title("Particle Library")]
        [ListDrawerSettings(ShowIndexLabels = true)]
        [SerializeField] private List<ParticleEntry> particles = new();

        private Dictionary<string, ParticleEntry> _particleMap;
        private IPoolService _poolService;

        public void Init()
        {
            _poolService = ServiceLocator.Get<IPoolService>();
            BuildParticleMap();
            WarmupAll();
        }

        private void BuildParticleMap()
        {
            _particleMap = new Dictionary<string, ParticleEntry>();

            foreach (var entry in particles)
            {
                if (string.IsNullOrEmpty(entry.name) || !entry.prefab)
                {
                    Debug.LogWarning("[ParticleController] Entry has missing name or prefab!");
                    continue;
                }

                if (!_particleMap.TryAdd(entry.name, entry))
                    Debug.LogWarning($"[ParticleController] Duplicate particle name: {entry.name}");
            }
        }

        private void WarmupAll()
        {
            foreach (var entry in _particleMap.Values)
                _poolService.Warmup(entry.prefab, entry.warmupCount, transform);
        }

        public void Play(string effectName, Vector3 position, Quaternion rotation)
        {
            Play(effectName, position, rotation, Color.white);
        }

        public void Play(string effectName, Vector3 position, Quaternion rotation, Color color)
        {
            if (_particleMap == null || !_particleMap.TryGetValue(effectName, out var entry))
            {
                Debug.LogWarning($"[ParticleController] Effect not found: {effectName}");
                return;
            }

            var instance = _poolService.Get(entry.prefab);
            instance.transform.SetPositionAndRotation(position, rotation);

            var main = instance.main;
            main.startColor = color;

            instance.Play(true);

            ReturnAfterPlay(instance).Forget();
        }

        private async UniTaskVoid ReturnAfterPlay(ParticleSystem instance)
        {
            var duration = instance.main.duration + instance.main.startLifetime.constantMax;

            await UniTask.Delay(
                TimeSpan.FromSeconds(duration),
                cancellationToken: this.GetCancellationTokenOnDestroy()
            );

            if (instance && !instance.isPlaying)
            {
                instance.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                _poolService.Return(instance);
            }
        }
    }
}



