using UnityEngine;

namespace LevelGeneration {
    public class LevelGenerator : MonoBehaviour {
        [SerializeField] private GameObject levelPrefab;

        [Space(30)]
        [Header("Level Length")] 
        [SerializeField] [Range(30, 50)] private float levelLengthMax;
        [SerializeField] [Range(30, 50)] private float levelLengthMin;

        [Space(30)] 
        [Header("Level width")] 
        [SerializeField] [Range(3, 10)] private float levelWidthMax;
        [SerializeField] [Range(3, 10)] private float levelWidthMin;

        [Space(30)] 
        [Header("Balls Count")] 
        [SerializeField] [Range(10, 30)] private int ballsCountMax;
        [SerializeField] [Range(10, 30)] private int ballsCountMin;

        [Space(30)] 
        [Header("Static Obstacles Count")] 
        [SerializeField] [Range(3, 10)] private int StaticObstaclesCountMax;
        [SerializeField] [Range(3, 10)] private int StaticObstaclesCountMin;
        [Header("Dynamic Obstacles Count")] 
        [SerializeField] [Range(3, 10)] private int DynamicObstaclesCountMax;
        [SerializeField] [Range(3, 10)] private int DynamicObstaclesCountMin;

        private float randomLength, randomWidth;
        private int randomBallsCount, randomStaticObstaclesCount, randomDynamicObstaclesCount;
        
        
        public void GenerateLevel() {
            CheckParametersCorrectness();
            GenerateRandomLevelProperties();
            
            
        }

        private void GenerateRandomLevelProperties() {
            randomLength = Random.Range(levelLengthMin, levelLengthMax);
            randomWidth = Random.Range(levelWidthMin, levelWidthMax);
            randomBallsCount = Random.Range(ballsCountMin, ballsCountMax + 1);
            randomStaticObstaclesCount = Random.Range(StaticObstaclesCountMin, StaticObstaclesCountMax + 1);
            randomDynamicObstaclesCount = Random.Range(DynamicObstaclesCountMin, DynamicObstaclesCountMax + 1);
        }

        private void CheckParametersCorrectness() {
            if (levelLengthMin > levelLengthMax) (levelLengthMax, levelLengthMin) = (levelLengthMin, levelLengthMax);
            if (levelWidthMin > levelWidthMax) (levelWidthMax, levelWidthMin) = (levelWidthMin, levelWidthMax);
            if (ballsCountMin > ballsCountMax) (ballsCountMax, ballsCountMin) = (ballsCountMin, ballsCountMax);
            if (StaticObstaclesCountMin > StaticObstaclesCountMax) (StaticObstaclesCountMax, StaticObstaclesCountMin) = (StaticObstaclesCountMin, StaticObstaclesCountMax);
            if (DynamicObstaclesCountMin > DynamicObstaclesCountMax) (DynamicObstaclesCountMax, DynamicObstaclesCountMin) = (DynamicObstaclesCountMin, DynamicObstaclesCountMax);
        }
    }
}