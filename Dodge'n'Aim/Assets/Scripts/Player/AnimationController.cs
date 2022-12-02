using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player {
    public class AnimationController : MonoBehaviour {
        public float floatDistance;
        public float floatDuration;

        private TweenerCore<Vector3, Vector3, VectorOptions> idleAnimation;
        
        
        private void Start() {
            //Invoke(nameof(StartIdleAnimation), Random.Range(0.0f, 0.3f));
        }

        private void StartIdleAnimation() {
            var originalY = transform.position.y;

            idleAnimation = transform.DOMoveY(originalY + floatDistance, floatDuration)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Yoyo);
        }
        
        public void StopIdleAnimation() {
            if (idleAnimation == null) CancelInvoke(nameof(StartIdleAnimation));
            else idleAnimation.Kill();
        }
    }
}