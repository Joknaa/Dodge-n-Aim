using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

namespace DefaultNamespace {
    public class EffectsController : MonoBehaviour{
        [Header("Shake")] public float shakeDuration;
        public float shakeMagnitude;
        public GameObject collectEffect;
        
        
        
        
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

        public void PlayCollectEffect(Transform parent) {
            Destroy(Instantiate(collectEffect, parent.position, Quaternion.identity), 1);
        }
    }
}