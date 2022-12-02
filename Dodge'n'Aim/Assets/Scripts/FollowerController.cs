using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace {
    public class FollowerController : MonoBehaviour {
        [Header("Idle parameters:")] 
        public float floatDistance;
        public float floatDuration;
        
        public float lerpSpeed = 0.5f;
        private GameObject _target;
        private Transform _targetTransform;

        private bool _isActivated;
        private TweenerCore<Vector3, Vector3, VectorOptions> idleAnimation;
        
        
        private IEnumerator Start() {
            var originalY = transform.position.y;

            yield return new WaitForSeconds(Random.Range(0.0f, 0.3f));
            
            idleAnimation = transform.DOMoveY(originalY + floatDistance, floatDuration)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Yoyo);
        }

        public void Init(Transform parent, GameObject target) {
            idleAnimation.Kill();
            
            Vector3 position = target.transform.position; // + new Vector3(0, 0, HasChickens ? forwardGap : 0);
            position.z -= FindObjectOfType<PlayerController>().listGap;
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

        private void Update() {
            if (!_isActivated) return;

            var targetPosition = _targetTransform.position;
            transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed);
        }
    }
}