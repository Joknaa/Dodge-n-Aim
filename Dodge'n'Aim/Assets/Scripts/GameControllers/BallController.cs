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
        public List<GameObject> balls = new List<GameObject>();
        public float BallsGap = 0.3f;
        
        private static GameObject headBall;
        private GameObject lastBall;

        private void Awake() => OnBallCollected += CollectBall;
        
        public void OnObstacleCollision() {
            
        }

        private void CollectBall(GameObject BallCollected) {
            BallCollected.tag = "Untagged";
            EffectsController.Instance.OnBallCollected.Invoke(BallCollected.transform);
            BallCollected.GetComponent<AnimationController>().StopIdleAnimation();
            BallCollected.GetComponent<MovementController>().InitializeNewBall(BallCollected.transform.parent, GetLastBall);
            balls.Add(BallCollected);
        }
        
        
        private bool HasBalls => balls.Count > 0;
        private GameObject GetLastBall => balls[balls.Count - 1];

        private void OnDestroy() {
            OnBallCollected -= CollectBall;
            instance = null;
        }
    }
}