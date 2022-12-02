using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace {
    public class InputController : MonoBehaviour {
        public event Action<Vector2> touchDownEvent;
        public event Action touchUpEvent;

        private Vector2 startPosition;
        private Vector2 endPosition;
        private Vector2 deltaPosition;

        private void Update() {
            if (Input.touchCount <= 0) {
                touchUpEvent?.Invoke();
                return;
            }

            Touch touch = Input.GetTouch(0);
            switch (touch.phase) {
                case TouchPhase.Began:
                    startPosition = touch.position;
                    break;
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    if (startPosition == Vector2.zero) startPosition = touch.position;
                    endPosition = touch.position;
                    deltaPosition = endPosition - startPosition;
                    touchDownEvent?.Invoke(deltaPosition);
                    startPosition = touch.position;
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (startPosition == Vector2.zero) startPosition = touch.position;
                    endPosition = touch.position;
                    deltaPosition = endPosition - startPosition;
                    touchDownEvent?.Invoke(deltaPosition);
                    startPosition = Vector2.zero;
                    endPosition = Vector2.zero;
                    
                    break;
            }

        }
    }
}