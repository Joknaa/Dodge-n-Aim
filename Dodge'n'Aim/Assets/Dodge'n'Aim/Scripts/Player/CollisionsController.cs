using System;
using GameControllers;
using GamePlayIngredients.FinishLine;
using UnityEngine;

namespace Player {
    public class CollisionsController : MonoBehaviour {
        
        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Ball") && !CompareTag("Ball")) BallController.OnBallCollected?.Invoke(other.transform);
            if (CompareTag("Head")) {
                if (other.CompareTag("Obstacle")) BallController.OnBallCollisionDetected?.Invoke(transform);
                if (other.CompareTag("CameraStop")) GameStateController.Instance.SetState(GameState.FinishLineSequence);
                if (other.CompareTag("GoThrough")) FinishLineController.OnFinishLineReached?.Invoke();
                if (other.CompareTag("GameOver")) FinishLineController.OnFinishLineMissed?.Invoke();
            }
        }
    }
}