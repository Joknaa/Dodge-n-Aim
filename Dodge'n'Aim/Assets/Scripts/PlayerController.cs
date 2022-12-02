using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace DefaultNamespace {
    public class PlayerController : MonoBehaviour {
        public List<GameObject> balls = new List<GameObject>();
        public CameraController cameraController;
        public float listGap = 0.3f;

        [Header("UI")] public GameObject gameWonUI;
        public GameObject gameOverIU;

        private static GameObject headBall;
        
        private GameObject lastBall;
        public EffectsController _effectsController;
        private bool isWon = false;

        private void Start() {
            // headBall = gameObject;
            balls.Add(gameObject);
        }

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.CompareTag("CameraStop")) PlayFinishLineSequence();
            if (other.gameObject.CompareTag("Ball")) Collect(other.gameObject);
            if (other.gameObject.CompareTag("GoThrough")) GameWon();
            if (other.gameObject.CompareTag("GameOver") && !isWon) GameOver();
        }

        private void GameOver() {
            gameOverIU.SetActive(true);
        }

        private void GameWon() {
            gameWonUI.SetActive(true);
            isWon = true;
        }

        private void PlayFinishLineSequence() {
            GetComponent<MovementController>().StartFinishLineSequence();
            cameraController.StopCameraMovement();
        }
        
        
        private void Collect(GameObject BallCollected) {
            //StartCoroutine(_effectsController.Shake(cameraController.gameObject));
            _effectsController.PlayCollectEffect(BallCollected.transform);
            BallCollected.GetComponent<FollowerController>().Init(transform.parent, GetLastBall);
            balls.Add(BallCollected);
        }
        
      
        
       
        private bool HasBalls => balls.Count > 0;
        private GameObject GetLastBall => balls[balls.Count - 1];

    }
}