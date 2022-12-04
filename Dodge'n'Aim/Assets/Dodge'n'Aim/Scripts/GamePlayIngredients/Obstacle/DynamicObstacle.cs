using System;
using DG.Tweening;
using GameControllers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GamePlayIngredients.Obstacles {
    public class DynamicObstacle : MonoBehaviour {
        [SerializeField] private float moveSpeed = 3;
        [SerializeField] private float moveDistance = 1f;


        private void Start() {
            Move();
        }


        public void Move() {
            var randomDirection = Random.Range(0, 2) == 0 ? Vector3.left : Vector3.right;
            var originalPosition = transform.position;
            var targetPosition = originalPosition + randomDirection * moveDistance;
            
            if (IsPositionOutsideLevel(targetPosition)) targetPosition = originalPosition - randomDirection * moveDistance;
            
            transform
                .DOMoveX(targetPosition.x, moveDistance/moveSpeed)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Yoyo);
        }
        
        private static bool IsPositionOutsideLevel(Vector3 position) => Math.Abs(position.x - GameManager.Instance.LevelWidth / 2) < 0.1f;
    }
}