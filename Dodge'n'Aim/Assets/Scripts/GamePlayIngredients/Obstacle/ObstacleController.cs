using GameControllers;
using UnityEngine;

namespace GamePlayIngredients.Obstacles {
    public class ObstacleController : MonoBehaviour {
        private void OnCollisionEnter(Collision other) {
            if (other.gameObject.CompareTag("CollectedBall")) {
                BallController.OnBallCollisionDetected?.Invoke(other.gameObject);
            }
        }
    }
}