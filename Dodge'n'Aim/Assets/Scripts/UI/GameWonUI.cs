using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameControllers.UISystem {
    public class GameWonUI : MonoBehaviour {
        public Button restartButton;
        
        private void Start() {
            restartButton.onClick.AddListener((() => SceneManager.LoadScene("StartMenu")));
        }
    }
}