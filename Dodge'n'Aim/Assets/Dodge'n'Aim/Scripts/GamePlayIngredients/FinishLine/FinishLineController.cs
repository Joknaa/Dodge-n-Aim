using System;
using System.Collections;
using DG.Tweening;
using GameControllers;
using LevelGeneration;
using OknaaEXTENSIONS;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GamePlayIngredients.FinishLine {
    public class FinishLineController : MonoBehaviour {
        public static Action OnFinishLineReached;
        public static Action OnFinishLineMissed;
        
        [SerializeField] private float rotationDuration;
        [SerializeField] private float rotationAngle;
        [SerializeField] private float moveDuration;

        
        // This value represents a ratio between the width of the object to be moved, and the width of the level.
        private const float _movementRangeToLevelWidthRatio = 3.25f;
        private float _levelWidth;
        private float _moveDistance;

        private void Awake() {
            OnFinishLineReached += ScoreGoal;
            OnFinishLineMissed += MissGoal;
        }
        
        private void Start() {
            _levelWidth = LevelGenerator.Instance.LevelWidth - 1;
            _moveDistance = _levelWidth - _movementRangeToLevelWidthRatio;
            Move();
        }

        private static void MissGoal() => GameStateController.Instance.SetState(GameState.GameLost);
        private static void ScoreGoal() => GameStateController.Instance.SetState(GameState.GameWon);

        

        private void Move() {
            var originalPosition = transform.position;
            transform.position = originalPosition.SetX(originalPosition.x - _moveDistance);
            print("From : " + transform.position + " to : " + originalPosition.x + _moveDistance);
            transform
                .DOMoveX(originalPosition.x + _moveDistance, moveDuration)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Yoyo);
        }
        
        private IEnumerator Rotate() {
            var forward = transform.forward;
            while (true) {
                yield return transform
                    .DORotate(forward * rotationAngle, rotationDuration)
                    .SetEase(Ease.Linear)
                    .SetLoops(-1, LoopType.Yoyo)
                    .WaitForCompletion();
            
                yield return transform
                    .DORotate(- forward * rotationAngle, rotationDuration)
                    .SetEase(Ease.Linear)
                    .SetLoops(-1, LoopType.Yoyo)
                    .WaitForCompletion();
            }
        }


        private void OnDestroy() {
            OnFinishLineReached -= ScoreGoal;
            OnFinishLineMissed -= MissGoal;
        }
    }
}