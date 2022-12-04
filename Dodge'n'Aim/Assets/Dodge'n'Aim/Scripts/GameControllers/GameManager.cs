using System;
using LevelGeneration;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameControllers {
    public class GameManager : MonoBehaviour {
        public static GameManager Instance => instance ??= FindObjectOfType<GameManager>();
        private static GameManager instance;

        [HideInInspector] public float LevelLength;
        [HideInInspector] public float LevelWidth;
        
        
        private void Awake() {
            GameStateController.Instance.SetState(GameState.Playing);
            LevelGenerator.Instance.RandomizeLevel();
        }
    }
}