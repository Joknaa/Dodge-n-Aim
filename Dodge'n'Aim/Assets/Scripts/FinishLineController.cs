using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace DefaultNamespace {
    public class FinishLineController : MonoBehaviour {
        public float rotationDuration;
        public float rotationAngle;
        
        private void Start() {
            // transform
            //     .DORotate(transform.forward * rotationAngle, rotationDuration)
            //     .SetEase(Ease.Linear)
            //     .SetLoops(-1, LoopType.Yoyo)
            //     .WaitForCompletion(); 
            StartCoroutine(Rotate());
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
        
        
    }
}