using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

    public StartScreen startScreen;
    public JoinScreen joinScreen;
    public LevelSelect levelSelect;
    public HostScreen hostScreen;

    void Start() {
        // HACK: Reset any pause state
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 1.0f;

        CustomNetworkManager mgr = GameObject.FindGameObjectWithTag("networkmanager").GetComponent<CustomNetworkManager>();

        if (mgr.IsClientConnected()) {
            hideStartScreen();
            displayLevelSelect();
        }
    }

    void Update() {

    }
	
    // START SCREEN
    public void displayStartScreen()
    {
        startScreen.gameObject.SetActive(true);
    }

    public void hideStartScreen()
    {
        startScreen.gameObject.SetActive(false);
    }

    // JOIN SCREEN
    public void displayJoinScreen()
    {
        joinScreen.gameObject.SetActive(true);
    }

    public void hideJoinScreen()
    {
        joinScreen.gameObject.SetActive(false);
    }

    // LEVEL SELECT
    public void displayLevelSelect()
    {
        levelSelect.gameObject.SetActive(true);
    }

    public void hideLevelSelect()
    {
        levelSelect.gameObject.SetActive(false);
    }

    // HOST SCREEN
    public void displayHostScreen()
    {
        hostScreen.gameObject.SetActive(true);
    }

    public void hideHostScreen()
    {
        hostScreen.gameObject.SetActive(false);
    }
}
