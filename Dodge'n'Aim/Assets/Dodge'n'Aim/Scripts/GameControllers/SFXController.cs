using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using GamePlayIngredients.FinishLine;
using OknaaEXTENSIONS;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameControllers {
    public class SFXController : MonoBehaviour {
        [SerializeField] private AudioSource audioSource;

        [Header("SFXs: ")]
        [SerializeField] private List<AudioClip> ballCollectSounds;
        [SerializeField] private List<AudioClip> ObstacleHitSounds;
        [SerializeField] private List<AudioClip> goalScoredSounds;
        [SerializeField] private List<AudioClip> goalMissedSounds;
        
        
        private void Awake() {
            BallController.OnBallCollected += Play_BallCollected;
            BallController.OnBallCollisionDetected += Play_ObstacleHit;
            FinishLineController.OnFinishLineReached += Play_GoalScored;
            FinishLineController.OnFinishLineMissed += Play_GoalMissed;
            
        }

        private void Play_BallCollected(Transform t) => audioSource.PlayOneShot(ballCollectSounds.Random());
        private void Play_ObstacleHit(Transform t) => audioSource.PlayOneShot(ObstacleHitSounds.Random());
        private void Play_GoalScored(Transform t) => audioSource.PlayOneShot(goalScoredSounds.Random());
        private void Play_GoalMissed() => audioSource.PlayOneShot(goalMissedSounds.Random()); 
        
        
        
        private void OnDestroy() {
            BallController.OnBallCollected -= Play_BallCollected;
            BallController.OnBallCollisionDetected -= Play_ObstacleHit;
            FinishLineController.OnFinishLineReached -= Play_GoalScored;
            FinishLineController.OnFinishLineMissed -= Play_GoalMissed;
        }
    }
}