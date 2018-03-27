using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameMenu : NetworkedObject {

    public Transform canvas;
    private bool isPaused;

    // Use this for initialization
    void Start() {
        canvas.gameObject.SetActive(false);
        isPaused = false;
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

        NetworkInteract();

    }

    public void ReturnToMainMenu() {
        LevelManager.Load("MainMenu");
    }

    public void Pause() {
        Time.timeScale = 0;
        isPaused = true;
    }

    public void Unpause() {
        Time.timeScale = 1;
        isPaused = false;
    }

    protected override void Interact(){
        if (isPaused){
            Unpause();
            Debug.Log("Game unpaused");
        }
        else{
            Pause();
            Debug.Log("Game Paused");
        }
    }
}
