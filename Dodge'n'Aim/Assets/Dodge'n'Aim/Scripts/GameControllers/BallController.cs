using System;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace GameControllers {
    public class BallController : MonoBehaviour {
        public static BallController Instance => instance ??= FindObjectOfType<BallController>();
        private static BallController instance;

        public static Action<Transform> OnBallCollected;
        public static Action<Transform> OnBallCollisionDetected;
        public static Action<Transform> OnBallScored;
        
        public List<Transform> balls = new List<Transform>();
        public float BallsGap = 0.6f;

        public static Transform headBall;

        private void Awake() {
            OnBallCollected += HandleBallCollecting;
            OnBallCollisionDetected += HandleBallObstacleCollision;
            OnBallScored += HandleBallScoring;
            headBall = GameObject.FindGameObjectWithTag("Head").transform;
        }

        private void HandleBallScoring(Transform scoredBall) {
            Destroy(scoredBall.gameObject);
        }

        private void HandleBallObstacleCollision(Transform collidedBall) {
            if (balls.Count <= 0) return;
            if (balls.Count == 1) {
                balls.Remove(collidedBall);
                Destroy(collidedBall.gameObject);
                GameStateController.Instance.SetState(GameState.GameLost);
                return;
            }

            var newHead = GetNewHead(collidedBall);
            newHead.tag = "Head";
            AdjustCameraTarget(newHead);
            balls.Remove(collidedBall);
            Destroy(collidedBall.gameObject);
        }

        private Transform GetNewHead(Transform collidedBall) {
            var nextBall = balls[balls.IndexOf(collidedBall) + 1];
            nextBall.GetComponent<MovementController>().SetIsHead(true);
            return nextBall;
        }

        private static void AdjustCameraTarget(Transform newTarget) => CameraController.OnCameraTargetChanged?.Invoke(newTarget);

        private void HandleBallCollecting(Transform collectedBall) {
            ConfigureCollectedBall(collectedBall);
            collectedBall.GetComponent<AnimationController>().StopIdleAnimation();
            collectedBall.GetComponent<MovementController>().InitializeNewBall(collectedBall.parent, HasBalls ? GetLastBall : headBall);
            balls.Add(collectedBall);
        }

        private void ConfigureCollectedBall(Transform collectedBall) {
            collectedBall.tag = "CollectedBall";

            Destroy(GetComponent<SphereCollider>());
            Destroy(GetComponent<Rigidbody>());
            Destroy(GetComponent<SphereCollider>());
        }


        private bool HasBalls => balls.Count > 0;
        private Transform GetLastBall => balls[balls.Count - 1];

        private void OnDestroy() {
            OnBallCollected -= HandleBallCollecting;
            OnBallCollisionDetected -= HandleBallObstacleCollision;
            instance = null;
        }
    }
}