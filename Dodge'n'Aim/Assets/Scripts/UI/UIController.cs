using System;
using UnityEngine;

namespace GameControllers.UISystem {
    public class UIController : MonoBehaviour {
        public GameObject gameWonUI;
        public GameObject gameOverIU;
        private bool isWon = false;

        private void Awake() {
            GameStateController.Instance.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnGameStateChanged() {
            var newGameState = GameStateController.Instance.GetState();
            switch (newGameState) {
                case GameState.GameWon:
                    OnGameWon();
                    break;
                case GameState.GameLost:
                    OnGameLost();
                    break;
            }
        }

        private void OnGameLost() {
            if (isWon) return;
            gameOverIU.SetActive(true);
        }

        private void OnGameWon() {
            gameWonUI.SetActive(true);
            isWon = true;
        }

        private void OnDestroy() {
            GameStateController.Instance.OnGameStateChanged -= OnGameStateChanged;
        }
    }
}