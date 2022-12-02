using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace {
    public class CameraController : MonoBehaviour {
        public GameObject player;
        public Vector3 positionOffSet;
        
        
        [Header("Shake")] public float shakeDuration;
        public float shakeMagnitude;


        
        private bool isStopped = false;

        private void Start() {
            positionOffSet = transform.position;
        }

        private void LateUpdate() {
            if (isStopped) return;
            
            var playerPosition = player.transform.position;
            playerPosition.x = 0;
            playerPosition.y = 0;
            
            transform.position = playerPosition + positionOffSet;
        }

        public void ShakeScreen() {
            StartCoroutine(Shake());
            IEnumerator Shake() {

                Vector3 originalOffset = positionOffSet;
                float elapsed = 0.0f;

                while (elapsed < shakeDuration) {
                    float x = Random.Range(-1f, 1f) * shakeMagnitude;
                    //float y = Random.Range(-1f, 1f) * shakeMagnitude;
                    //float z = Random.Range(-1, 1) * shakeMagnitude;

                    positionOffSet = new Vector3(x, originalOffset.y, originalOffset.z);
                    elapsed += Time.deltaTime;

                    yield return new WaitForEndOfFrame();
                }
                positionOffSet = originalOffset;
            }
            
        }
        
        
        public void StopCameraMovement() {
            isStopped = true;
        }
    }
}