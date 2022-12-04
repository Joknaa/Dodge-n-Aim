using System.Collections.Generic;
using GameControllers;
using LevelGeneration;
using UnityEngine;
using UnityEngine.UIElements;

namespace Player {
    public class MovementController : MonoBehaviour {
        private static int FramesCount = 0;
        [SerializeField] private bool isHead = false;

        [Header("Head Parameters")] 
        [SerializeField] private float forwardSpeed;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float finishLineForce;

        [Header("Follower Parameters")] 
        [SerializeField] private float lerpSpeed = 0.5f;

        private Transform _target;
        private Transform _targetTransform;

        private bool _isActivated;
        private bool _touching;
        private float _touchDeltaX;
        private float _ballsGap;
        private List<Transform> _ballsList;
        private float _levelWidth;


        private void Awake() {
            InputController.touchDownEvent += OnTouchDown;
            InputController.touchUpEvent += OnTouchUp;
            GameStateController.Instance.OnGameStateChanged += OnGameStateChange;

            if (isHead) SetupHeadBall();
        }

        private void Start() {
            _ballsGap = BallController.Instance.BallsGap;
            _ballsList = BallController.Instance.balls;
            _levelWidth = LevelGenerator.Instance.LevelWidth - 1;
        }

        private void SetupHeadBall() {
            _isActivated = true;
            tag = "Head";
            BallController.Instance.balls.Add(transform);
        }

        private void Update() {
            if (!_isActivated) return;

            Rotate();
            if (isHead) HandleHeadMovement();
            else HandleFollowerMovement();
        }

        private void Rotate() {
            transform.Rotate(Vector3.right * (rotationSpeed * Time.deltaTime));
        }

        private void HandleHeadMovement() {
            transform.Translate(Vector3.forward * (forwardSpeed * Time.deltaTime), Space.World);

            if (!_touching) return;
            
            var deltaPositionX = DeltaTouch2DeltaPosition(_touchDeltaX);
            var targetPosition = transform.position + deltaPositionX * Vector3.right;
            targetPosition.x = Mathf.Clamp(targetPosition.x, -_levelWidth * 0.5f, _levelWidth * 0.5f);
            transform.position = targetPosition;
        }

        private void HandleFollowerMovement() {
            var targetPosition = _targetTransform.position;
            targetPosition.z -= _ballsGap;
            transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed);
        }

        private float DeltaTouch2DeltaPosition(float touchDelta) {
            var screenWidth = Screen.width;
            var touchDeltaScreenRatio = touchDelta / screenWidth;

            var levelWidth = 3;
            var targetPositionAfterTouch = levelWidth * touchDeltaScreenRatio;
            return targetPositionAfterTouch;
        }


        public void InitializeNewBall(Transform parent, Transform target) {
            Vector3 position = target.position;
            position.z -= _ballsGap;
            transform.position = position;
            transform.SetParent(parent);
            
            _target = target;
            _targetTransform = target.transform;

            _isActivated = true;
        }


        private void OnGameStateChange() {
            if (!isHead) return;
            if (GameStateController.Instance.GetState() == GameState.FinishLineSequence) StartFinishLineSequence();
        }

        private void StartFinishLineSequence() {
            GetComponent<Rigidbody>().AddForce(finishLineForce * Vector3.forward, ForceMode.Impulse);
        }

        private void OnTouchDown(Vector2 position) {
            _touching = true;
            _touchDeltaX = position.x;
        }

        private void OnTouchUp() {
            _touching = false;
        }

        private void OnDestroy() {
            InputController.touchDownEvent -= OnTouchDown;
            InputController.touchUpEvent -= OnTouchUp;
            GameStateController.Instance.OnGameStateChanged -= OnGameStateChange;
        }

        public void SetIsHead(bool flag) => isHead = flag;
    }
}