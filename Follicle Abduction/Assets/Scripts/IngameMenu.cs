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
    public Slider musicSlider;
    private bool isPaused;

    public override void Start() {
        base.Start();
        canvas.gameObject.SetActive(false);
        isPaused = false;

        // Load master volume settings from playerprefs
        if (PlayerPrefs.HasKey("masterVolume")){
            volumeSlider.value = PlayerPrefs.GetFloat("masterVolume");
            AudioListener.volume = volumeSlider.value;
        }
        else{
            // Default to current scene master volume
            PlayerPrefs.SetFloat("masterVolume", 1.0f);
        }

        // Load music volume settings from playerprefs
        AudioSource bgmSource = GameObject.FindWithTag("bgm").GetComponent<AudioSource>();
        if (PlayerPrefs.HasKey("musicVolume")){
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
            bgmSource.volume = musicSlider.value;
        }
        else{
            // Default to current scene music volume
            PlayerPrefs.SetFloat("musicVolume", 1.0f);
        }

    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            ViewMenu();
        }
    }

    // Allows user to view the ingame menu (also pauses the game when opened)
    public void ViewMenu() {
        // if (!canvas.gameObject.activeInHierarchy) {
        //     canvas.gameObject.SetActive(true);
        // }
        // else {
        //     canvas.gameObject.SetActive(false);
        // }
        NetworkInteract();
    }

    public void ReturnToMainMenu() {
        networkManager.NetworkLoadScene("MainMenu");
    }

    public void Pause() {
        Time.timeScale = 0;
        isPaused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        //check if this player is human
        CustomNetworkManager NetworkManager = GameObject.FindWithTag("networkmanager").GetComponent<CustomNetworkManager>();
        if (NetworkManager.myRole == "human") {
            GameObject human = GameObject.FindWithTag("playerA");
            human.GetComponent<FirstPersonController>().enabled = false;
        }
    }

    public void Unpause() {
        Time.timeScale = 1;
        isPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        PlayerPrefs.Save();
        //check if this player is human?
        CustomNetworkManager NetworkManager = GameObject.FindWithTag("networkmanager").GetComponent<CustomNetworkManager>();
        if (NetworkManager.myRole == "human") {
            GameObject human = GameObject.FindWithTag("playerA");
            human.GetComponent<FirstPersonController>().enabled = true;
        }
    }

    // Syncs pause/unpause game status with other player
    protected override void Interact(){
        if (isPaused){
            Unpause();
            canvas.gameObject.SetActive(false);
            Debug.Log("Game unpaused");
        }
        else{
            Pause();
            canvas.gameObject.SetActive(true);
            Debug.Log("Game Paused");
        }
    }

    // Allows users to adjust volume via the ingame menu slider
    public void changeVolumeSlider() {
        AudioListener.volume = volumeSlider.value;
        PlayerPrefs.SetFloat("masterVolume", volumeSlider.value);
    }

    public void changeMusicVolumeSlider(){
        AudioSource bgmSource = GameObject.FindWithTag("bgm").GetComponent<AudioSource>();
        bgmSource.volume = musicSlider.value;
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
    }
}
