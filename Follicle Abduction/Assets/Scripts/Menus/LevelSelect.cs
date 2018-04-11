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

    public bool hostPlayerReady = false;
    public bool clientPlayerReady = false;
    public bool gameStarted = false;

    bool levelSelected = false;
    public bool isHost;

    public class LevelSelectMessage : MessageBase {
        public static short type = MsgType.Highest + 2;
        public int level;
    }

    public class RoleSelectMessage : MessageBase {
        public static short type = MsgType.Highest + 3;
        public bool host;
        public string role;
    }

    public class PlayerReadyMessage : MessageBase {
        public static short type = MsgType.Highest + 4;
        public bool isHostPlayer;
        public bool isReady;
    }

	// Use this for initialization
	void Start () 
	{
        manager = GameObject.FindGameObjectWithTag("networkmanager").GetComponent<CustomNetworkManager>();
        isHost = manager.isTheHost();

        // Register the handlers for synchronizing state
        if(isHost) {
            NetworkServer.RegisterHandler(LevelSelectMessage.type, OnLevelSelect);
            NetworkServer.RegisterHandler(RoleSelectMessage.type, OnRoleSelect);
            NetworkServer.RegisterHandler(PlayerReadyMessage.type, OnPlayerReady);
        } else {
            manager.client.RegisterHandler(LevelSelectMessage.type, OnLevelSelect);
            manager.client.RegisterHandler(RoleSelectMessage.type, OnRoleSelect);
            manager.client.RegisterHandler(PlayerReadyMessage.type, OnPlayerReady);
        }

        back.onClick.AddListener(backButton);

		if (lvl1_unlocked) 
		{
			lvl1.onClick.AddListener (() => {
                LevelSelectMessage msg = new LevelSelectMessage();
                msg.level = 1;
                sendSyncMessage(LevelSelectMessage.type, msg);
                selectLevel(1);
            });
		} 
		else 
		{
			lvl1.interactable = false;
		}
			
		if (lvl2_unlocked) 
		{
			lvl2.onClick.AddListener (() => {
                LevelSelectMessage msg = new LevelSelectMessage();
                msg.level = 2;
                sendSyncMessage(LevelSelectMessage.type, msg);
                selectLevel(2);
            });
		} 
		else 
		{
			lvl2.interactable = false;
		}

		if (lvl3_unlocked) 
		{
			lvl3.onClick.AddListener (() => {
                LevelSelectMessage msg = new LevelSelectMessage();
                msg.level = 3;
                sendSyncMessage(LevelSelectMessage.type, msg);
                selectLevel(3);
            });
		} 
		else 
		{
			lvl3.interactable = false;
		}

		startBtn.onClick.AddListener (() => {
            PlayerReadyMessage msg = new PlayerReadyMessage();
            msg.isReady = true;
            msg.isHostPlayer = isHost;
            sendSyncMessage(PlayerReadyMessage.type, msg);

            // Set it locally
            setReady(isHost, true);
        });

        if(isHost)
        {
            roleSelectClient.interactable = false;
            roleSelectHost.onValueChanged.AddListener(delegate {
                // Send the message
                RoleSelectMessage msg = new RoleSelectMessage();
                msg.host = true;
                msg.role = roleSelectHost.value == 0 ? "human" : "alien";
                sendSyncMessage(RoleSelectMessage.type, msg);
            });
        }
        else
        {
            roleSelectHost.interactable = false;
            roleSelectClient.onValueChanged.AddListener(delegate {
                RoleSelectMessage msg = new RoleSelectMessage();
                msg.host = false;
                msg.role = roleSelectClient.value == 0 ? "human" : "alien";
                sendSyncMessage(RoleSelectMessage.type, msg);
            });
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

        if(hostPlayerReady && clientPlayerReady && !gameStarted)
        {
            startGame();
        }
    }

    private void selectLevel(int level) {
        border1.color = notSelected;
        border2.color = notSelected;
        border3.color = notSelected;
        levelSelected = true;

        switch(level) {
            case 1:
                border1.color = selected;
                selectedLevel = "Level_1";
                break;
            case 2:
                border2.color = selected;
                selectedLevel = "Level_2";
                break;
            case 3:
                border3.color = selected;
                selectedLevel = "Level_3";
                break;
        }
    }

    // TODO this could be cleaner
    private void selectRole(bool isHost, string role) {
        Debug.Log("Role Selected: " + isHost + " -- " + role);
        Dropdown dd;
        if(isHost) {
            dd = roleSelectHost;
        } else {
            dd = roleSelectClient;
        }

        switch(role) {
            case "alien":
                dd.value = 1;
            break;
            case "human":
                dd.value = 0;
            break;
        }
    }

    private void setReady(bool isHostPlayer, bool isReady) {
        // TODO ready / start game logic
        // Ideally we should have an indicator when each player is ready/isn't
        // We should also have it so that if anything changes, the ready state is reset
        // The lead player can start the game if both players are ready
        Debug.Log((isHostPlayer ? "host" : "client") + " " + (isReady? "is" : "isnt") + " ready");

        if(isHostPlayer && isReady)
        {
            hostPlayerReady = true;
        }
        else if(isReady)
        {
            clientPlayerReady = true;
        }

    }

    private void sendSyncMessage(short messageTypeId, MessageBase msg) {
        if(isHost) {
            NetworkServer.SendToAll(messageTypeId, msg);
        } else {
            manager.client.Send(messageTypeId, msg);
        }
    }

	private void OnLevelSelect(NetworkMessage netMsg) {
		LevelSelectMessage lsMsg = netMsg.ReadMessage<LevelSelectMessage>();
        selectLevel(lsMsg.level);
	}
    
    private void OnRoleSelect(NetworkMessage networkMessage) {
        RoleSelectMessage rsMsg = networkMessage.ReadMessage<RoleSelectMessage>();
        selectRole(rsMsg.host, rsMsg.role);
    }

    private void OnPlayerReady(NetworkMessage networkMessage) {
        PlayerReadyMessage prMsg = networkMessage.ReadMessage<PlayerReadyMessage>();

        setReady(prMsg.isHostPlayer, prMsg.isReady);
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
