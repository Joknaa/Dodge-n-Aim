using System;
using UnityEngine;
using UnityEngine.Pool;
using System.Threading.Tasks;
using OknaaEXTENSIONS;
using Object = UnityEngine.Object;

namespace LevelGeneration {
    public static class Pool_Ball {
        private static ObjectPool<GameObject> _pool;
        private static bool _isInit;
        private const bool _collectionCheck = true;
        private const int _capacity = 40;
        private const int _maxCapacity = 100;

        private static GameObject _ballPrefab;
        private static Transform _parent;
        private static Vector3 _position;
        

        public static void Init(GameObject ball) {
            if (_isInit) return;
            
            _ballPrefab = ball;
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
            _position.y = _ballPrefab.transform.position.y;
            return Object.Instantiate(_ballPrefab, _position, Quaternion.identity, _parent);
        }
        
        
        private static void OnGet(GameObject pooled) {
            pooled.transform.parent = _parent;
            var position = pooled.transform.position;
            pooled.transform.position = position.SetXZ(_position.x, _position.z);
            pooled.SetActive(true);
        }
        
        private static void OnRelease(GameObject pooled) {
            if (pooled == null) return;
            pooled.SetActive(false);
        }
        
        private static void OnDestroy(GameObject pooled) {
            Object.DestroyImmediate(pooled);
        }
    }
}