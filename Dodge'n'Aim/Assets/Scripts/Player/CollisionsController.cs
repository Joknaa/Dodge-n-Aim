using GameControllers;
using UnityEngine;

namespace Player {
    public class CollisionsController : MonoBehaviour {
        
        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.CompareTag("CameraStop")) GameStateController.Instance.SetState(GameState.FinishLineSequence);
            if (other.gameObject.CompareTag("Ball")) BallController.OnBallCollected?.Invoke(other.gameObject);
            if (other.gameObject.CompareTag("GoThrough")) GameStateController.Instance.SetState(GameState.GameWon);
            if (other.gameObject.CompareTag("GameOver")) GameStateController.Instance.SetState(GameState.GameLost);
        }
    }
}