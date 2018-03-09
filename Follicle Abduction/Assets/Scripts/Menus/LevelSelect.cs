using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour {

    public MenuManager menuManager;

	public Button startBtn;
	public Button lvl1;
	public Button lvl2;
	public Button lvl3;
    public Button back;

	public bool lvl1_unlocked = true;
	public bool lvl2_unlocked = true;
	public bool lvl3_unlocked = false;

	public Image border1;
	public Image border2;
	public Image border3;

	Color selected = new Color(52f/255f,152f/255f,219f/255f,1f);
	Color notSelected = new Color(1f,1f,1f,0f);

	int selectedLevel = 0;

	// Use this for initialization
	void Start () 
	{
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
	}


	void selectedScene1()
	{
		selectedLevel = 1;

		border1.color = selected;
		border2.color = notSelected;
		border3.color = notSelected;

		startBtn.interactable = true;
	}

	void selectedScene2()
	{
		selectedLevel = 2;

		border1.color = notSelected;
		border2.color = selected;
		border3.color = notSelected;

		startBtn.interactable = true;
	}

	void selectedScene3()
	{
		selectedLevel = 3;

		border1.color = notSelected;
		border2.color = notSelected;
		border3.color = selected;

		startBtn.interactable = true;
	}

	void startGame()
	{
		Debug.Log ("Load scene " + selectedLevel);
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
    }
}
