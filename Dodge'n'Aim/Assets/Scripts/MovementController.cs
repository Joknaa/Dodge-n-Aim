using DefaultNamespace;
using UnityEngine;

public class MovementController : MonoBehaviour {
    public float forwardSpeed;
    public float finishLineForce;
    
    private bool touching;
    private float touchDeltaX;

    
    private InputController _inputController;
    
    
    // todo: add offset to the movement of the balls
    // todo: add obstacles
    // todo: add the balls into one big parent instead of following the head
    // todo: adjust camera
    
    
    private void Start() {
        _inputController = FindObjectOfType<InputController>();
        _inputController.touchDownEvent += OnTouchDown;
        _inputController.touchUpEvent += OnTouchUp;
    }

    private void Update() {
        transform.Translate(Vector3.forward * (forwardSpeed * Time.fixedDeltaTime));
        var currentPosition = transform.position;
        currentPosition.x = Mathf.Clamp(currentPosition.x, -1.5f, 1.5f);
        transform.position = currentPosition;
        
        
        if (!touching) return;

        var deltaPositionX = DeltaTouch2DeltaPosition(touchDeltaX);
        var targetPosition = transform.position + deltaPositionX * Vector3.right;
        targetPosition.x = Mathf.Clamp(targetPosition.x, -1.5f, 1.5f);
        transform.position = targetPosition;
    }

    private float DeltaTouch2DeltaPosition(float touchDelta) {
        var screenWidth = Screen.width;
        var touchDeltaScreenRatio = touchDelta / screenWidth;

        var levelWidth = 3;
        var targetPositionAfterTouch = levelWidth * touchDeltaScreenRatio;
        return targetPositionAfterTouch;
    }

    public void StartFinishLineSequence() {
        // forwardSpeed *= 3;
        GetComponent<Rigidbody>().AddForce(finishLineForce * Vector3.forward, ForceMode.Impulse);
    }

    private void OnTouchDown(Vector2 position) {
        touching = true;
        touchDeltaX = position.x;
    }

    private void OnTouchUp() {
        touching = false;
    }

    private void OnDestroy() {
        _inputController.touchDownEvent -= OnTouchDown;
        _inputController.touchUpEvent -= OnTouchUp;
    }
}