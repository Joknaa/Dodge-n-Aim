using System;
using System.Collections;
using Cinemachine;
using OknaaEXTENSIONS;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameControllers {
    public class CameraController : MonoBehaviour {
        public static Action<Transform> OnCameraTargetChanged;
        
        private CinemachineVirtualCamera virtualCamera;


        private void Awake() {
            GameStateController.Instance.OnGameStateChanged += OnGameStateChanged;
            OnCameraTargetChanged += UpdateTarget;
            virtualCamera = GetComponent<CinemachineVirtualCamera>();
        }
        
        private void Start() {
            // UpdateTarget(GameObject.FindGameObjectWithTag("Head").transform);
            UpdateTarget(BallController.headBall.transform);
        }


        private void OnGameStateChanged() {
            if (GameStateController.Instance.GetState() == GameState.FinishLineSequence) PlayFinishLineSequence();
        }
        private void UpdateTarget(Transform newTarget) => virtualCamera.m_Follow = newTarget;


        private void PlayFinishLineSequence() => virtualCamera.m_Follow = null;

        private void OnDestroy() {
            GameStateController.Instance.OnGameStateChanged -= OnGameStateChanged;
            OnCameraTargetChanged -= UpdateTarget;
        }
    }
}