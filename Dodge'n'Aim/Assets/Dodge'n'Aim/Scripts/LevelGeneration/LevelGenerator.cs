using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using GD.MinMaxSlider;
using UnityEditor;
using Random = UnityEngine.Random;

namespace LevelGeneration {
    public class LevelGenerator : MonoBehaviour {
        public static LevelGenerator Instance => instance ??= FindObjectOfType<LevelGenerator>();
        private static LevelGenerator instance;
        
        private static int _seed;

        [Header("=> Prefabs")] [Space(5)] [SerializeField]
        private GameObject groundPrefab;

        [Space(5)] [SerializeField] private Transform ballsParent;
        [SerializeField] private GameObject ballPrefab;

        [Space(5)] [SerializeField] private Transform staticObstacleParent;
        [SerializeField] private GameObject staticObstaclePrefab;
        [SerializeField] private Transform dynamicObstacleParent;
        [SerializeField] private GameObject DynamicObstaclePrefab;

        [Space(5)] 
        [SerializeField] private Transform FinishLineAncorPoint;
        [SerializeField] private Transform FinishLine;

        [Space(20)] [Header("Level Settings")] [SerializeField] [MinMaxSlider(20, 100)]
        private Vector2 levelLength = new Vector2(30f, 50f);

        [Space(10)] [SerializeField] [MinMaxSlider(1, 8)]
        private Vector2 levelWidth = new Vector2(4f, 10f);

        [Space(10)] [SerializeField] [MinMaxSlider(10, 30)]
        private Vector2Int ballsCount = new Vector2Int(10, 30);

        [Space(10)] [SerializeField] [MinMaxSlider(0, 10)]
        private Vector2Int StaticObstaclesCount = new Vector2Int(0, 10);

        [SerializeField] [MinMaxSlider(0, 10)] private Vector2Int DynamicObstaclesCount = new Vector2Int(0, 10);

        private readonly List<GameObject> pooledBalls = new List<GameObject>();
        private readonly List<GameObject> pooledStaticObstacles = new List<GameObject>();
        private readonly List<GameObject> pooledDynamicObstacles = new List<GameObject>();
        private const float _levelBoundOffset_MaxX = 1f;
        private const float _levelBoundOffset_MinX = 1f;
        private const float _levelBoundOffset_MaxZ = 10f;
        private const float _levelBoundOffset_MinZ = 5f;

        private float _randomLength;
        private float _randomWidth;
        private int _randomBallsCount;
        private int _randomStaticObstaclesCount;
        private int _randomDynamicObstaclesCount;

        private Vector3 _groundMaxBounds;
        private Vector3 _groundMinBounds;
        
        private bool _isLevelGenerated;
        
        
        /// <summary>
        /// Generates the level
        /// </summary>
        /// <param name="forceGeneration"></param>
        public void RandomizeLevel(bool forceGeneration = false) {
            if (!forceGeneration && _isLevelGenerated) return; 
            Reset();
            InitializePools();
            GenerateRandomLevelProperties();
            ApplyGeneratedProperties();
            _seed = Random.Range(0, int.MaxValue);
            _isLevelGenerated = true;
        }

        /// <summary>
        /// Deletes all the generated ingredients, and clears the object pools
        /// </summary>
        public void Reset() {
            Pool_Ball.Dispose();
            Pool_StaticObstacle.Dispose();
            Pool_DynamicObstacle.Dispose();

            ResetBallsPool();
            ResetStaticObstaclePool();
            ResetDynamicObstaclePool();
            ClearPooledGameObjects(ballsParent);
            ClearPooledGameObjects(staticObstacleParent);
            ClearPooledGameObjects(dynamicObstacleParent);
            _isLevelGenerated = false;


            void ResetBallsPool() {
                // if (pooledBalls.Count == 0) return;

                foreach (var pooledBall in pooledBalls) {
                    Pool_Ball.Release(pooledBall, true);
                }

                pooledBalls.Clear();
                Pool_Ball.Dispose();
            }
            void ResetStaticObstaclePool() {
                // if (pooledStaticObstacles.Count == 0) return;

                foreach (var pooled in pooledStaticObstacles) {
                    Pool_StaticObstacle.Release(pooled, true);
                }

                Pool_StaticObstacle.Dispose();
                pooledStaticObstacles.Clear();
            }
            void ResetDynamicObstaclePool() {
                // if (pooledDynamicObstacles.Count == 0) return;

                foreach (var pooled in pooledDynamicObstacles) {
                    Pool_DynamicObstacle.Release(pooled, true);
                }

                Pool_DynamicObstacle.Dispose();
                pooledDynamicObstacles.Clear();
            }

            static void ClearPooledGameObjects(Transform parent) {
                foreach (Transform child in parent) {
                    DestroyImmediate(child.gameObject);
                }
            }
        }
#if UNITY_EDITOR

        public void SaveLevel() {
            if (!Directory.Exists("Assets/Dodge'n'Aim/Prefabs/Levels")) {
                AssetDatabase.CreateFolder("Assets", "Dodge'n'Aim");
                AssetDatabase.CreateFolder("Assets/Dodge'n'Aim", "Prefabs");
                AssetDatabase.CreateFolder("Assets/Dodge'n'Aim/Prefabs", "Levels");
            }
            string localPath = "Assets/Dodge'n'Aim/Prefabs/Levels/Level_" + _seed + ".prefab";

            // localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);
            print("Saving level to: " + localPath);
            
            PrefabUtility.SaveAsPrefabAsset(gameObject, localPath, out var prefabSuccess);
            Debug.Log(prefabSuccess ? "Prefab was saved successfully" : "Prefab was not saved");
        }
        
#endif
        
        private void InitializePools() {
            Pool_Ball.Init(ballPrefab);
            Pool_StaticObstacle.Init(staticObstaclePrefab);
            Pool_DynamicObstacle.Init(DynamicObstaclePrefab);
        }

        private void GenerateRandomLevelProperties() {
            _randomLength = GetRandomValue(levelLength);
            _randomWidth = GetRandomValue(levelWidth);
            _randomBallsCount = GetRandomValue(ballsCount);
            _randomStaticObstaclesCount = GetRandomValue(StaticObstaclesCount);
            _randomDynamicObstaclesCount = GetRandomValue(DynamicObstaclesCount);
        }


        private void ApplyGeneratedProperties() {
            groundPrefab.transform.localScale = new Vector3(_randomWidth, 0.2f, _randomLength);

            CalculateLevelBounds();
            GenerateBalls();
            GenerateStaticObstacles();
            GenerateDynamicObstacles();
            GenerateFinishLine();
        }

        private void GenerateFinishLine() {
            FinishLine.position = FinishLineAncorPoint.position;
        }

        private void GenerateDynamicObstacles() {
            for (var i = 0; i < _randomDynamicObstaclesCount; i++) {
                var spawnPoint = GetRandomPointWithingGroundBounds();
                pooledDynamicObstacles.Add(Pool_DynamicObstacle.Get(dynamicObstacleParent, spawnPoint));
            }
        }

        private void GenerateStaticObstacles() {
            for (var i = 0; i < _randomStaticObstaclesCount; i++) {
                var spawnPoint = GetRandomPointWithingGroundBounds();
                pooledStaticObstacles.Add(Pool_StaticObstacle.Get(staticObstacleParent, spawnPoint));
            }
        }

        private void CalculateLevelBounds() {
            var groundScale = groundPrefab.transform.localScale;
            var groundCenter = groundPrefab.transform.position + groundScale.z / 2 * Vector3.forward;

            _groundMaxBounds = new Vector3(
                groundCenter.x + groundScale.x / 2 - _levelBoundOffset_MaxX, 
                groundCenter.y + groundScale.y / 2, 
                groundCenter.z + groundScale.z / 2 - _levelBoundOffset_MaxZ);
            _groundMinBounds = new Vector3(
                groundCenter.x - groundScale.x / 2 + _levelBoundOffset_MinX, 
                groundCenter.y - groundScale.y / 2, 
                groundCenter.z - groundScale.z / 2 + _levelBoundOffset_MinZ);
        }

        private void GenerateBalls() {
            for (var i = 0; i < _randomBallsCount; i++) {
                var spawnPoint = GetRandomPointWithingGroundBounds();
                pooledBalls.Add(Pool_Ball.Get(ballsParent, spawnPoint));
            }
        }

        private Vector3 GetRandomPointWithingGroundBounds() {
            var x = Random.Range(_groundMinBounds.x, _groundMaxBounds.x);
            var z = Random.Range(_groundMinBounds.z, _groundMaxBounds.z);
            return new Vector3(x, 0, z);
        }


        public void ClearPooledObjects() {
            Pool_Ball.Dispose();
            Pool_StaticObstacle.Dispose();
            Pool_DynamicObstacle.Dispose();
        }


        private static int GetRandomValue(Vector2Int vector) => Random.Range(vector.x, vector.y);
        private static float GetRandomValue(Vector2 vector) => Random.Range(vector.x, vector.y);

        public float LevelWidth => groundPrefab.transform.localScale.x;

        private void OnDestroy() {
            _isLevelGenerated = false;
            Reset();
            instance = null;
        }
    }
}