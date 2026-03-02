using System;
using System.Collections.Generic;
using _Game.Scripts.Pool;
using _Game.Scripts.Services;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.Audio
{
    [Serializable]
    public class SoundClip
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
        [Range(0.5f, 2f)] public float pitch = 1f;
    }

    public class AudioService : MonoBehaviour, IAudioService
    {
        [Title("Sound Library")]
        [ListDrawerSettings(ShowIndexLabels = true)]
        [SerializeField] private List<SoundClip> sounds = new();

        [Title("Pool")]
        [SerializeField] private AudioSource audioSourcePrefab;
        [SerializeField] private int warmupCount = 5;

        private Dictionary<string, SoundClip> _soundMap;
        private IPoolService _poolService;

        public void Init()
        {
            _poolService = ServiceLocator.Get<IPoolService>();
            BuildSoundMap();
            _poolService.Warmup(audioSourcePrefab, warmupCount, transform);
        }

        private void BuildSoundMap()
        {
            _soundMap = new Dictionary<string, SoundClip>();

            foreach (var sound in sounds)
            {
                if (string.IsNullOrEmpty(sound.name) || !sound.clip)
                {
                    Debug.LogWarning("[AudioService] Sound entry has missing name or clip!");
                    continue;
                }

                if (!_soundMap.TryAdd(sound.name, sound))
                    Debug.LogWarning($"[AudioService] Duplicate sound name: {sound.name}");
            }
        }

        public void Play(string clipName)
        {
            if (_soundMap == null || !_soundMap.TryGetValue(clipName, out var sound))
            {
                Debug.LogWarning($"[AudioService] Sound not found: {clipName}");
                return;
            }

            var source = _poolService.Get(audioSourcePrefab, transform);
            source.clip = sound.clip;
            source.volume = sound.volume;
            source.pitch = sound.pitch;
            source.Play();

            ReturnAfterPlay(source, sound.clip.length / sound.pitch).Forget();
        }

        private async UniTask ReturnAfterPlay(AudioSource source, float duration)
        {
            await UniTask.Delay(
                TimeSpan.FromSeconds(duration),
                cancellationToken: this.GetCancellationTokenOnDestroy()
            );

            if (source && !source.isPlaying)
                _poolService.Return(source);
        }
    }
}





