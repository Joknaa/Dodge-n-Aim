using System;
using UnityEngine;

namespace GameControllers {
    public class GameManager : MonoBehaviour {
        private void Awake() {
            GameStateController.Instance.SetState(GameState.Playing);
        }
    }
}