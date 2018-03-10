using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameMenu : MonoBehaviour {

    public Transform canvas;

    // Use this for initialization
    void Start() {
        canvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            ViewMenu();
        }
    }

    public void ViewMenu() {
        if (!canvas.gameObject.activeInHierarchy) {
            canvas.gameObject.SetActive(true);
        }
        else {
            canvas.gameObject.SetActive(false);
        }
    }

    public void ReturnToMainMenu() {
        LevelManager.Load("MainMenu");
    }

    public void Pause() {
        Time.timeScale = 0;
    }

    public void Unpause() {
        Time.timeScale = 1;
    }
}
