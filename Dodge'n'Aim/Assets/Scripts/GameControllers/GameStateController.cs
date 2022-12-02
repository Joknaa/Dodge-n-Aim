using System;

namespace GameControllers {
    
    public enum GameState {
        None,
        Playing,
        Paused,
        FinishLineSequence,
        GameWon,
        GameLost
    }

    public class GameStateController {
        public static GameStateController Instance => instance ??= new GameStateController();
        private static GameStateController instance;
        public event Action OnGameStateChanged;
        private GameState currentGameState { get; set; } = GameState.None;

        public void SetState(GameState newState) {
            if (currentGameState == newState) return;
            currentGameState = newState;
            OnGameStateChanged?.Invoke();
        }

        public GameState GetState() => currentGameState;
    }
}