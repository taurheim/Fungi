using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;

/* 
    In-game pause menu.
 */

public class IngameMenu : NetworkedObject {

    public Transform canvas;
    public Slider volumeSlider;
    private bool isPaused;

    void Start() {
        canvas.gameObject.SetActive(false);
        isPaused = false;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            ViewMenu();
        }
    }

    // Allows user to view the ingame menu (also pauses the game when opened)
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
        Cursor.visible = true;
        LevelManager.Load("MainMenu");
    }

    public void Pause() {
        Time.timeScale = 0;
        isPaused = true;
        Cursor.visible = true;
        //check if this player is human?
        GameObject human = GameObject.FindWithTag("playerA");
        if (human) {
            human.GetComponent<FirstPersonController>().enabled = false;
        }
    }

    public void Unpause() {
        Time.timeScale = 1;
        isPaused = false;
        Cursor.visible = false;
        //check if this player is human?
        GameObject human = GameObject.FindWithTag("playerA");
        if (human) {
            human.GetComponent<FirstPersonController>().enabled = true;
        }
    }

    // Syncs pause/unpause game status with other player
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

    // Allows users to adjust volume via the ingame menu slider
    public void changeVolumeSlider() {
        AudioListener.volume = volumeSlider.value;
    }
}
