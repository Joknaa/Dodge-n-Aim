using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameControllers {
    public class VFXController : MonoBehaviour {
        [Header("Shake")]
        public float shakeDuration;
        public float shakeMagnitude;
        public GameObject BallCollectedEffect;


        private void Awake() {
            BallController.OnBallCollected += PlayCollectEffect;
        }

        private void PlayCollectEffect(Transform parent) => Destroy(Instantiate(BallCollectedEffect, parent.position, Quaternion.identity), 1);
        
        
        
        public IEnumerator Shake(GameObject target) {
            var targetTransform = target.transform;
            Vector3 originalPosition = targetTransform.position;
            float elapsed = 0.0f;

            while (elapsed < shakeDuration) {
                float x = Random.Range(-1f, 1f) * shakeMagnitude;
                //float y = Random.Range(-1f, 1f) * shakeMagnitude;
                //float z = Random.Range(-1, 1) * shakeMagnitude;

                targetTransform.position = new Vector3(x, originalPosition.y, originalPosition.z);
                elapsed += Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }
            targetTransform.position = new Vector3(originalPosition.x, originalPosition.y, targetTransform.position.z);
        }

        private void OnDestroy() {
            BallController.OnBallCollected -= PlayCollectEffect;
        }
    }
}