using OknaaEXTENSIONS;
using UnityEngine;

namespace LevelGeneration {
    public class Entourage : MonoBehaviour {
        public Transform LeftSeating;
        public Transform RightSeating;

        public void UpdateSeatingPosition(Vector3 ancorPosition, float levelWidth) {
            transform.position = ancorPosition;
            
            var leftSeatingPosition = LeftSeating.localPosition;
            leftSeatingPosition.x = -levelWidth / 2;
            LeftSeating.localPosition = leftSeatingPosition;
            
            var rightSeatingPosition = RightSeating.localPosition;
            rightSeatingPosition.x = levelWidth / 2;
            RightSeating.localPosition = rightSeatingPosition;
        }
    }
}