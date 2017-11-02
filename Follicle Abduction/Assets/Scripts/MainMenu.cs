using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public Button startGame;
	public Button quit;

	public Dropdown dropdown;

	// Use this for initialization
	void Start () 
	{
		startGame.onClick.AddListener (loadScene);
		quit.onClick.AddListener (quitGame);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void loadScene()
	{
		if (dropdown.value == 0) 
		{
			SceneManager.LoadScene ("Demo_level");
		}
		else if (dropdown.value == 1) 
		{
			//load level 1
		}
		else if (dropdown.value == 2) 
		{
			//load level 2
		}
		else if (dropdown.value == 3) 
		{
			//load level 3
		}
	}

	void quitGame()
	{
		Application.Quit ();
	}
}
