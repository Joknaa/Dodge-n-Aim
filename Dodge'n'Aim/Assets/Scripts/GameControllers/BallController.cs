using System;
using System.Collections.Generic;
using DefaultNamespace;
using Player;
using UnityEngine;

namespace GameControllers {
    public class BallController : MonoBehaviour {
        public static BallController Instance => instance ??= FindObjectOfType<BallController>();
        private static BallController instance;

        public static Action<GameObject> OnBallCollected;
        public static Action<GameObject> OnBallCollisionDetected;
        public List<GameObject> balls = new List<GameObject>();
        public float BallsGap = 0.3f;

        private static GameObject headBall;
        private GameObject lastBall;

        private void Awake() {
            OnBallCollected += HandleBallCollecting;
            OnBallCollisionDetected += HandleBallObstacleCollision;
        }

        private void HandleBallObstacleCollision(GameObject collidedBall) {
            if (balls.Count <= 0) return;
            if (balls.Count == 1) {
                balls.Remove(collidedBall);
                Destroy(collidedBall);
                GameStateController.Instance.SetState(GameState.GameLost);
                return;
            }

            var newHead = GetNewHead(collidedBall);
            AdjustCameraTarget(newHead);
            balls.Remove(collidedBall);
            Destroy(collidedBall);
        }

        private GameObject GetNewHead(GameObject collidedBall) {
            var nextBall = balls[balls.IndexOf(collidedBall) + 1];

            nextBall
                .GetComponent<MovementController>()
                .SetIsHead(true);

            return nextBall;
        }

        private void AdjustCameraTarget(GameObject newTarget) {
            CameraController.OnCameraTargetChanged?.Invoke(newTarget.transform);
        }

        private void HandleBallCollecting(GameObject collectedBall) {
            ConfigureCollectedBall(collectedBall);
            EffectsController.Instance.OnBallCollected.Invoke(collectedBall.transform);
            collectedBall.GetComponent<AnimationController>().StopIdleAnimation();
            collectedBall.GetComponent<MovementController>().InitializeNewBall(collectedBall.transform.parent, GetLastBall);
            balls.Add(collectedBall);
        }

        private void ConfigureCollectedBall(GameObject collectedBall) {
            collectedBall.tag = "CollectedBall";

            Destroy(GetComponent<SphereCollider>());
            Destroy(GetComponent<Rigidbody>());
            Destroy(GetComponent<SphereCollider>());
        }


        private bool HasBalls => balls.Count > 0;
        private GameObject GetLastBall => balls[balls.Count - 1];

        private void OnDestroy() {
            OnBallCollected -= HandleBallCollecting;
            OnBallCollisionDetected -= HandleBallObstacleCollision;
            instance = null;
        }
    }
}