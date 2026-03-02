using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts.Core.Pool
{
    public class PoolService : MonoBehaviour, IPoolService
    {
        private readonly Dictionary<int, Queue<Component>> _pools = new();
        private readonly Dictionary<int, int> _prefabMap = new();

        public T Get<T>(T prefab, Transform parent = null) where T : Component
        {
            var key = prefab.GetInstanceID();

            if (_pools.TryGetValue(key, out var pool) && pool.Count > 0)
            {
                var instance = (T)pool.Dequeue();
                instance.gameObject.SetActive(true);
                
                if (parent) instance.transform.SetParent(parent);
                
                return instance;
            }

            var newInstance = Instantiate(prefab, parent ? parent : transform);
            _prefabMap[newInstance.GetInstanceID()] = key;
            return newInstance;
        }

        public void Return<T>(T instance) where T : Component
        {
            var instanceId = instance.GetInstanceID();

            if (!_prefabMap.TryGetValue(instanceId, out var prefabKey))
            {
                Debug.LogWarning($"[PoolService] Unknown instance returned, destroying: {instance.name}");
                Destroy(instance.gameObject);
                return;
            }

            instance.gameObject.SetActive(false);
            instance.transform.SetParent(transform);

            if (!_pools.ContainsKey(prefabKey))
                _pools[prefabKey] = new Queue<Component>();

            _pools[prefabKey].Enqueue(instance);
        }

        public void Warmup<T>(T prefab, int count, Transform parent = null) where T : Component
        {
            var key = prefab.GetInstanceID();

            if (!_pools.ContainsKey(key))
                _pools[key] = new Queue<Component>();

            for (int i = 0; i < count; i++)
            {
                var instance = Instantiate(prefab, parent ? parent : transform);
                instance.gameObject.SetActive(false);
                _prefabMap[instance.GetInstanceID()] = key;
                _pools[key].Enqueue(instance);
            }
        }
    }
}

