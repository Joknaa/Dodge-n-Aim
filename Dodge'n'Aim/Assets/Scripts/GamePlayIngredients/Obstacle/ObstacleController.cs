using GameControllers;
using UnityEngine;

namespace GamePlayIngredients.Obstacles {
    public class ObstacleController : MonoBehaviour {
        private void OnCollisionEnter(Collision collision) {
            if (collision.gameObject.CompareTag("Player")) {
                collision.gameObject.GetComponent<BallController>().OnObstacleCollision();
            }
        }
    }
}