using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameControllers.UISystem {

    public class StartMenuUI : MonoBehaviour {
        private void Start() {
            GetComponentInChildren<Button>().onClick.AddListener(() => { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); });
        }
    }
}