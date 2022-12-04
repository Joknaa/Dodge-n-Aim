using System;
using UnityEngine;
using UnityEngine.Pool;
using System.Threading.Tasks;
using OknaaEXTENSIONS;
using Object = UnityEngine.Object;

namespace LevelGeneration {
    public static class Pool_DynamicObstacle {
        private static ObjectPool<GameObject> _pool;
        private static bool _isInit;
        private const bool _collectionCheck = true;
        private const int _capacity = 10;
        private const int _maxCapacity = 20;

        private static GameObject _obstaclePrefab;
        private static Transform _parent;
        private static Vector3 _position;


        public static void Init(GameObject ball) {
            if (_isInit) return;

            _obstaclePrefab = ball;
            _pool = new ObjectPool<GameObject>(Create, OnGet, OnRelease, OnDestroy, _collectionCheck, _capacity, _maxCapacity);
            _isInit = true;
        }

        public static GameObject Get(Transform parent, Vector3 position) {
            _parent = parent;
            _position = position;
            return _pool.Get();
        }

        public static void Release(GameObject pooled, bool destroy = false) {
            _pool.Release(pooled);
            if (destroy) Object.DestroyImmediate(pooled);
        }

        public static void Dispose() => _pool?.Dispose();

        private static GameObject Create() {
            _position.y = _obstaclePrefab.transform.position.y;
            var rotation = _obstaclePrefab.transform.rotation;
            return Object.Instantiate(_obstaclePrefab, _position, rotation, _parent);
        }


        private static void OnGet(GameObject pooled) {
            pooled.transform.parent = _parent;
            var position = pooled.transform.position;
            pooled.transform.position = position.SetXZ(_position.x, _position.z);
            pooled.SetActive(true);
        }

        private static void OnRelease(GameObject pooled) {
            pooled.SetActive(false);
        }

        private static void OnDestroy(GameObject pooled) {
            Object.DestroyImmediate(pooled);
        }
    }
}