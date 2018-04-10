using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class LocalPlayerData : Photon.PunBehaviour {

	//Local Data

	public string Animal;
	public string Special;
	public string Win;
	public string Lose;
	public string Environment;
	public bool sky;
	public int Level;
	public string playerName;
	public Sprite Icon;
    PlayerUI UI;
    public GameObject PlayerUiPrefab;

    public SetupLocalPlayer setup;
    //

    //	int tempID;
    public static GameObject LocalPlayerInstance;


    //public LocalServerInfo serverInfo;
    public enum Species
	{
		Chameleon = 0, Crocodile = 1, Crow = 2, Deer = 3, Eagle = 4, EgyptianPlover = 5,
		Hyena = 6, Lion = 7, Mallard = 8, Mouse = 9, Otter = 10, Rabit = 11, Snake = 12 

	};
    private void Awake()
    {
        // #Important
        // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
        if (photonView.isMine)
        {
            LocalPlayerData.LocalPlayerInstance = this.gameObject;
        }
        // #Critical
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(this.gameObject);
    }
    
    private void Start()
    {
        if (photonView.isMine)
        {
            if (PlayerUiPrefab != null)
            {
                GameObject _uiGo = Instantiate(PlayerUiPrefab) as GameObject;
                _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
                UI = _uiGo.GetComponent<PlayerUI>();
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
                setupLocalData();
            }
            else
            {
                Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.", this);
            }
        }


    }


    public void SetPeeks(int peekNum)
	{
		UI.numPeeksLeft = peekNum;

		if(peekNum == 2)
		{
            UI.hasTwoPeeks = true;
		}
		else if(peekNum == 1)
		{
            UI.hasTwoPeeks = false;
		}
	}


	public void setupLocalData()
	{
		
			PlayerDataBank.instance.RequestPlayerData (setup.playerID, out Animal,out Special, out Win,out Lose,out Environment,out sky,out Level, out Icon);
            playerName = photonView.owner.NickName ;
			UI.localCard.SetCard (Animal, Special, Win, Lose, Environment, Icon);
			UI.PeekButton.SetActive (true);
		if (UI.isTurtle) {
			LoadCrowButton ();
		} else 
		{
			DisableCrowButton ();
		}
		if (UI.isOctopus) {
			LoadChameleonButton ();
		} else 
		{
			DisableChameleonButton();
		}

		if(!UI.isOctopus && !UI.isTurtle)
		{
			setup.CmdChangePeekStatus (true);
		}
        GetComponent<SetupLocalPlayer>().Init(playerName);


    }
    public void LoadChameleonButton()
	{
      //  UI.octopusButton = GameObject.Find ("OctopusButton");
        UI.octopusButton.SetActive (true);
	}
	public void LoadCrowButton()
	{
		//CrowButton= GameObject.Find ("TurtleButton");

		if(UI.turtleButton != null)
		{
            UI.turtleButton.SetActive (true);

		}
	}
	public void DisableChameleonButton()
	{
		//ChameleonButton = GameObject.Find ("OctopusButton");

		if(UI.octopusButton != null)
		{
            UI.octopusButton.SetActive (false);

		}
	}
	public void DisableCrowButton()
	{
		//CrowButton= GameObject.Find ("TurtleButton");

		if(UI.turtleButton != null)
		{
			UI.turtleButton.SetActive (false);

		}
	}
	public void LoadPeekData(int IDToPeek)
	{
		
			if(UI.numPeeksLeft > 0)
			{
				if(UI.numPeeksLeft == 1)
				{
					PlayerDataBank.instance.RequestPlayerData (IDToPeek, out Animal,out Special, out Win,out Lose,out Environment,out sky,out Level, out Icon);

                UI.cardOne.SetCard (Animal, Special, Win, Lose, Environment, Icon);
                UI.PeekButton.SetActive (false);
                UI.PeekOneRefButton.SetActive (true);

				if (UI.hasTwoPeeks) {
                    UI.PeekOneText.text = "Peek Info 2 (" + photonView.owner.NickName + ")";

				} else 
				{
                    UI.PeekOneText.text = "Peek Info 1 (" + photonView.owner.NickName + ")";

				}
				setup.CmdSetPeekDoneStatus (true);
				}
				else if(UI.numPeeksLeft == 2)
				{

                UI.hasTwoPeeks = true;
			
				PlayerDataBank.instance.RequestPlayerData (IDToPeek, out Animal,out Special, out Win,out Lose,out Environment,out sky,out Level, out Icon);


                UI.cardTwo.SetCard (Animal, Special, Win, Lose, Environment, Icon);
                UI.PeekTwoRefButton.SetActive (true);
               // UI.PeekTwoText.text = "Peek Info 1 (" + serverInfo. (IDToPeek) + ")";


				}

				UI.numPeeksLeft--;
			}

	}
	public void LoadPeekData(int IDToPeek, string nameToLoad)
	{

		if(UI.numPeeksLeft > 0)
		{
			if(UI.numPeeksLeft == 1)
			{
				PlayerDataBank.instance.RequestPlayerData (IDToPeek, out Animal,out Special, out Win,out Lose,out Environment,out sky,out Level, out Icon);

                UI.cardOne.SetCard (Animal, Special, Win, Lose, Environment, Icon);
                UI.PeekButton.SetActive (false);
                UI.PeekOneRefButton.SetActive (true);
				if (UI.hasTwoPeeks) {
                    UI.PeekOneText.text = "Peek Info 2 (" + nameToLoad + ")";

				} else 
				{
                    UI.PeekOneText.text = "Peek Info 1 (" + nameToLoad + ")";

				}
				setup.CmdSetPeekDoneStatus (true);
			}
			else if(UI.numPeeksLeft == 2)
			{


				PlayerDataBank.instance.RequestPlayerData (IDToPeek, out Animal,out Special, out Win,out Lose,out Environment,out sky,out Level, out Icon);


                UI.cardTwo.SetCard (Animal, Special, Win, Lose, Environment, Icon);
                UI.PeekTwoRefButton.SetActive (true);
                UI.PeekTwoText.text = "Peek Info 1 (" + nameToLoad + ")";


			}

            UI.numPeeksLeft--;
		}

	}

    
}
