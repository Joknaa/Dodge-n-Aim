using DefaultNamespace;
using GameControllers;
using OknaaEXTENSIONS;
using UnityEngine;

namespace Player {
    public class MovementController : MonoBehaviour {
        [SerializeField] private bool isHead = false;

        [Space(10)] [Header("Head Parameters")] [SerializeField]
        private float forwardSpeed;

        [SerializeField] private float finishLineForce;

        [Space(10)] [Header("Follower Parameters")] [SerializeField]
        private float lerpSpeed = 0.5f;

        private GameObject _target;
        private Transform _targetTransform;

        private bool _isActivated;
        private bool _touching;
        private float _touchDeltaX;
        private float ballsGap;


        private void Awake() {
            InputController.touchDownEvent += OnTouchDown;
            InputController.touchUpEvent += OnTouchUp;
            GameStateController.Instance.OnGameStateChanged += OnGameStateChange;
        }

        private void Start() {
            if (isHead) SetupHeadBall();
            ballsGap = BallController.Instance.BallsGap;
        }

        private void SetupHeadBall() {
            _isActivated = true;
            tag = "Untagged";
            BallController.Instance.balls.Add(gameObject);
            GetComponent<AnimationController>().StopIdleAnimation();
        }

        private void Update() {
            if (!_isActivated) return;

            if (isHead) HandleHeadMovement();
            else HandleFollowerMovement();
        }

        private void HandleHeadMovement() {
            transform.Translate(Vector3.forward * (forwardSpeed * Time.deltaTime));

            if (!_touching) return;

            var deltaPositionX = DeltaTouch2DeltaPosition(_touchDeltaX);
            var targetPosition = transform.position + deltaPositionX * Vector3.right;
            targetPosition.x = Mathf.Clamp(targetPosition.x, -1.5f, 1.5f);
            transform.position = targetPosition;
        }
        
        private void HandleFollowerMovement() {
            var targetPosition = _targetTransform.position;
            targetPosition.z -= ballsGap;
            transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed);
        }

        private float DeltaTouch2DeltaPosition(float touchDelta) {
            var screenWidth = Screen.width;
            var touchDeltaScreenRatio = touchDelta / screenWidth;

            var levelWidth = 3;
            var targetPositionAfterTouch = levelWidth * touchDeltaScreenRatio;
            return targetPositionAfterTouch;
        }


        public void InitializeNewBall(Transform parent, GameObject target) {
            Vector3 position = target.transform.position;
            position.z -= ballsGap;
            transform.position = position;
            transform.SetParent(parent);
            tag = "Untagged";

            Destroy(GetComponent<SphereCollider>());
            Destroy(GetComponent<Rigidbody>());
            Destroy(GetComponent<SphereCollider>());

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
    }
}