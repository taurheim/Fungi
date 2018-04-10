using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LevelSelect : MonoBehaviour {

    public MenuManager menuManager;
    public CustomNetworkManager manager;

	public Button startBtn;
	public Button lvl1;
	public Button lvl2;
	public Button lvl3;
    public Button back;

	public bool lvl1_unlocked = true;
	public bool lvl2_unlocked = true;
	public bool lvl3_unlocked = true;

	public Image border1;
	public Image border2;
	public Image border3;

	Color selected = new Color(52f/255f,152f/255f,219f/255f,1f);
	Color notSelected = new Color(1f,1f,1f,0f);

	public string selectedLevel = "test";

    public Dropdown roleSelectHost;
    public Dropdown roleSelectClient;

    public bool localPlayerReady = false;
    public bool partnerPlayerReady = false;
    public bool gameStarted = false;

    bool levelSelected = false;
    public bool isHost;

	// Use this for initialization
	void Start () 
	{
        manager = GameObject.FindGameObjectWithTag("networkmanager").GetComponent<CustomNetworkManager>();
        isHost = manager.isTheHost();

        back.onClick.AddListener(backButton);

		if (lvl1_unlocked) 
		{
			lvl1.onClick.AddListener (selectedScene1);
		} 
		else 
		{
			lvl1.interactable = false;
		}
			
		if (lvl2_unlocked) 
		{
			lvl2.onClick.AddListener (selectedScene2);
		} 
		else 
		{
			lvl2.interactable = false;
		}

		if (lvl3_unlocked) 
		{
			lvl3.onClick.AddListener (selectedScene3);
		} 
		else 
		{
			lvl3.interactable = false;
		}

		startBtn.onClick.AddListener (startGame);

        if(isHost)
        {
            roleSelectClient.interactable = false;
        }
        else
        {
            roleSelectHost.interactable = false;
        }
	}

    void Update()
    {
        if(roleSelectHost.value == roleSelectClient.value)
        {
            startBtn.interactable = false;
        }
        else if(levelSelected)
        {
            startBtn.interactable = true;
        }
    }


	void selectedScene1()
	{
        selectedLevel = "Level_1";

        border1.color = selected;
		border2.color = notSelected;
		border3.color = notSelected;

        levelSelected = true;
	}

	void selectedScene2()
	{
		selectedLevel = "Level_2";

		border1.color = notSelected;
		border2.color = selected;
		border3.color = notSelected;

        levelSelected = true;
    }

	void selectedScene3()
	{
        selectedLevel = "Level_3";

        border1.color = notSelected;
		border2.color = notSelected;
		border3.color = selected;

        levelSelected = true;
    }

    public void startGame()
    {
        gameStarted = true;
        Cursor.visible = false;

        string role = "";
        if (isHost)
        { 
            if (roleSelectHost.value == 0)
            {
                role = "human";
            }
            else if (roleSelectHost.value == 1)
            {
                role = "alien";
            }
        }
        else
        {
            if (roleSelectClient.value == 0)
            {
                role = "human";
            }
            else if (roleSelectClient.value == 1)
            {
                role = "alien";
            }
        }

        manager.myRole = role;
        loadScene();
    }

    void backButton()
    {
        breakConnection();
        menuManager.hideLevelSelect();
        menuManager.displayStartScreen();
    }

    void breakConnection()
    {
        // End connection between players when one leaves the level select screen
        manager.StopAllCoroutines();
    }


    public bool loadScene()
    {
        manager.NetworkLoadScene(selectedLevel);
        return true;
    }
}
