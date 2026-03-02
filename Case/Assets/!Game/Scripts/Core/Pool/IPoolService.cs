using _Game.Scripts.Services;
using UnityEngine;

namespace _Game.Scripts.Core.Pool
{
    public interface IPoolService : IService
    {
        public T Get<T>(T prefab, Transform parent = null) where T : Component;
        public void Return<T>(T instance) where T : Component;
        public void Warmup<T>(T prefab, int count, Transform parent = null) where T : Component;
    }
}

