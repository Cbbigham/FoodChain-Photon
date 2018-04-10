using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LocalServerInfo : Photon.PunBehaviour, IPunObservable {


	public bool isInitialized;
   
	public Dropdown menuOne;
	public Dropdown ChameleonMenu;
	public Dropdown CrowMenu;

	public Dropdown mins;
	public Dropdown secs;

	public Dropdown LocalNearbyPlayers;

	public SetupLocalPlayer selfData;

	public TextNotification PeekNotificationText;

	public Button[] HabitatButtons;
	public Camera[] HabitatCameras;

	public GameObject[] HabitatSquares;


	public GameObject attackObject;

	public GameObject hostControl;

	public Text LocalText;
	public GlobalChatManager chatManager;

	public PeekMenuToggle combatMenu;
	public PeekMenuToggle environmentMenu;

	public ServerLivingStatus lifeStats;
	public GameObject deathPanel;

	public Text deathPanlText;
	public string starvationSpecificText;
	public WinnerList winers;

	public int lastTeleport;

	public bool hasTeleported;
	public GameObject peekButton;

	public List<int> TeleportHistory;

	public enum Species
	{
		Chameleon = 0, Crocodile = 1, Crow = 2, Deer = 3, Eagle = 4, EgyptianPlover = 5,
		Hyena = 6, Lion = 7, Mallard = 8, Mouse = 9, Otter = 10, Rabit = 11, Snake = 12 

	};

	void Start () {
		isInitialized = false;
		lastTeleport = -1;
		hasTeleported = false;

	}
	
	void Update () {

     /*

		if(Time.timeSinceLevelLoad > 0 && Time.timeSinceLevelLoad < 10f)
		{
			if(!PlayerDataBank.instance.getSkyStatus(selfData.playerID))
			{
				HabitatButtons [1].interactable = false;
			}	
			else if(PlayerDataBank.instance.getSkyStatus(selfData.playerID))
			{
				HabitatButtons [1].interactable = true;
			}
		}
        */
	}

	public void FillNearbyList()
	{
		if(selfData != null)
		{
			List<SetupLocalPlayer> nearbyPlayer = selfData.pList.returnNearbyPlayers ();
			List<string> playerNames = new List<string> ();
			foreach(SetupLocalPlayer p in nearbyPlayer)
			{
				playerNames.Add (p.pName);
			}

			LocalNearbyPlayers.options.Clear ();
			LocalNearbyPlayers.AddOptions (playerNames);
			
		}
	
	
	}


	public void DetermineButtonState(int ID)
	{

		if(isHomeHabitat(ID))
		{
			activateAllNonHomeHabitats ();
		}
		else if(!isHomeHabitat(ID))
		{
			deactivateAllNonHomeHabitats ();
		}

		if(!PlayerDataBank.instance.getSkyStatus(selfData.playerID))
		{
			HabitatButtons [1].interactable = false;
		}
			
	}

	void activateAllNonHomeHabitats()
	{
		for(int i = 0; i < HabitatButtons.Length; i++)
		{
			if (i != 1) {
				HabitatButtons [i].interactable = true;

			} else if(PlayerDataBank.instance.getSkyStatus(selfData.playerID)) 
			{
				HabitatButtons [i].interactable = true;

			}
		}

	}
	void deactivateAllNonHomeHabitats()
	{
		for(int i = 0; i < HabitatButtons.Length; i++)
		{
			if (!isHomeHabitat (i + 1)) {
				HabitatButtons [i].interactable = false;
			} else 
			{
				HabitatButtons [i].interactable = true;
			}
		}
	}
	bool isHomeHabitat(int id)
	{
		bool retval = false;

		int home = PlayerDataBank.instance.RequestPlayerHabitat(selfData.playerID);

		if(home == id)
		{
			retval = true;
		}

		return retval;
	}


	public void ActivateCamera(int camToActivate)
	{
		for(int i = 0; i < HabitatCameras.Length; i++)
		{
			if (i == camToActivate) {
				HabitatCameras [i].enabled = true;
			} else {

				HabitatCameras [i].enabled = false;

			}
		}

	}


	public void TeleportPlayerToEnvironment(int ID)
	{
		hasTeleported = true;

		lastTeleport = ID;
		//Id is either 1,2,3 or 4

		AudioManager.instance.ChangeAmbianceTrack (ID- 1);

		//take in 0,1,2,3
		Vector3 center = HabitatSquares [ID-1].transform.position;
		selfData.MoveSelfToRandomPointInEnvironment (center);

		//take in 1,2,3,4
		ActivateCamera (ID);

		if(environmentMenu.active)
		{
			environmentMenu.ToggleMenu ();
		}

		if(combatMenu.active)
		{
			combatMenu.ToggleMenu ();
		}

	}
	public void PerformRandomSelfTeleport()
	{
		hasTeleported = true;

		bool[] validAreas = new bool[4]{false,false,false,false};

		if (lastTeleport == -1) {

			validAreas[0] = true;

			if (PlayerDataBank.instance.getSkyStatus (selfData.playerID)) {
				validAreas [1] = true;
			} else 
			{
				validAreas [1] = false;
			}

			validAreas [2] = true;
			validAreas [3] = true;

		} 
		else 
		{
			if(isHomeHabitat(lastTeleport))
			{
				validAreas[0] = true;

				if (PlayerDataBank.instance.getSkyStatus (selfData.playerID)) {
					validAreas [1] = true;
				} else 
				{
					validAreas [1] = false;

				}

				validAreas [2] = true;
				validAreas [3] = true;
			}
			else
			{
//				int home = 0;

				for(int i = 1; i < 5; i++)
				{
					if(isHomeHabitat(i))
					{
					//	home = i;
						validAreas [i - 1] = true;
					}
				}
					
			}

		}
		 
		List<int> options = new List<int> ();
	
		for(int i = 0; i < validAreas.Length; i++)
		{
			if(validAreas[i])
			{
				options.Add (i);
			}
		}


		int element = Random.Range (0, options.Count);
		int ID = 0;


		ID = options [element];

		
			
		if(selfData.isAlive)
		{
			HabitatButtons [ID].onClick.Invoke ();

		}

	}


	public bool AllAreDonePeeking()
	{
		
		bool retval = true;
        SetupLocalPlayer[] players = FindObjectsOfType<SetupLocalPlayer>();
		for (int i = 0; i < players.Length; i++)
		{
			if(!players[i].donePeeking)
			{
				retval = false;
			}
		}

		return retval;
	}

	public void InitializeServerNameList()
	{
		if(isInitialized == false)
		{
			//reshuffle (availableNames);
			//orderedNameList = availableNames;
			fillDropdownMenu ();
		}
	

		isInitialized = true;

	}
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {

        }
        else
        {

        }
    }
	List<int> WhosAliveOutOfMallardRabbitDearAndOtter()
	{
		List<int> survivors = new List<int> ();
        SetupLocalPlayer[] players = FindObjectsOfType<SetupLocalPlayer>();
		foreach(SetupLocalPlayer player in players)
		{
			if(player.playerID == (int)Species.Mallard || player.playerID == (int)Species.Rabit || player.playerID == (int)Species.Deer || player.playerID == (int)Species.Otter)
			{
				if(player.isAlive)
				{
					survivors.Add (player.playerID);
				}
			}
		}
		return survivors;
	}

	bool areAllSurvivorsCurrentlyInMyBubble()
	{
		bool retval = true;

		List<SetupLocalPlayer> playersInMyBubble = selfData.GetComponentInChildren<NearbyPlayerList> ().nearbyPlayers;

		List<int> allSurvivors = WhosAliveOutOfMallardRabbitDearAndOtter ();
		List<int> survivorsInMyBubble = new List<int> ();

		foreach(SetupLocalPlayer p in playersInMyBubble)
		{
			if(p.playerID == (int)Species.Mallard || p.playerID == (int)Species.Rabit || p.playerID == (int)Species.Deer || p.playerID == (int)Species.Otter)
			{
				survivorsInMyBubble.Add (p.playerID);

			}
				
		}

		foreach(int i in allSurvivors)
		{
			if(!survivorsInMyBubble.Contains(i))
			{
				retval = false;
			}
		}

		return retval;
	}
		
	public void handleSelfDeath()
	{
		selfData.isAlive = false;
		selfData.CmdSetAliveStatus (false);
		selfData.CmdCreateGrave ();
		deathPanel.SetActive (true);

        /*if (selfData.isServer)
		{
			hostControl.SetActive (true);
		}
        */
	}
	public void DisplayStarvationText()
	{
		deathPanlText.text = starvationSpecificText;
	}
	public bool ReturnLifeStatusOfPlayerWithID(int ID)
	{
		bool status = false;
        SetupLocalPlayer[] players = FindObjectsOfType<SetupLocalPlayer>();
		foreach(SetupLocalPlayer player in players)
		{
			if(player != null)
			{
				if(player.playerID == ID)
				{
					status = player.isAlive;
				}
			}
		}

		return status;
	
	}

	public int ReturnNumberOfDeadPlayers()
	{
		int numDead = 0;
        SetupLocalPlayer[] players = FindObjectsOfType<SetupLocalPlayer>();

        foreach (SetupLocalPlayer player in players)
		{
			if (player == null) {
				numDead++;
			} else if(!player.isAlive)
			{
				numDead++;
			}
		}

		return numDead;

	}
	public void checkPlayerForWin()
	{

		bool isWinner = false;

		switch(selfData.playerID)
		{
		case (int)Species.Chameleon:

			isWinner = selfData.isAlive ? true : false;
			break;
		case (int)Species.Crocodile:
			if(selfData.isAlive)
			{
				isWinner = true;
			}

			break;
		case (int)Species.Crow:
			break;
		case (int)Species.Deer:
			isWinner = selfData.isAlive ? true : false;

			break;
		case (int)Species.Eagle:
			isWinner = selfData.isAlive ? true : false;

			break;
		case (int)Species.EgyptianPlover:

			/*
			if(ReturnLifeStatusOfPlayerWithID((int)Species.Crocodile))
			{
				isWinner = true;
			}

*/
			break;
		case (int)Species.Hyena:
			/*
			if(ReturnLifeStatusOfPlayerWithID((int)Species.Lion) && selfData.isAlive)
			{
				isWinner = true;	
			}
			*/
			break;
		case (int)Species.Lion:
			if(selfData.isAlive)
			{
				isWinner = true;
			}
						
			break;
		case (int)Species.Mallard:
			isWinner = selfData.isAlive ? true : false;

			break;
		case (int)Species.Mouse:
			/*
			if(ReturnLifeStatusOfPlayerWithID((int)Species.Lion) == true)
			{
				isWinner = true;	
			}
			*/
			break;
		case (int)Species.Otter:
			isWinner = selfData.isAlive ? true : false;

			break;
		case (int)Species.Rabit:
			isWinner = selfData.isAlive ? true : false;

			break;
		case (int)Species.Snake:

			selfData.CmdCheckSnakeWin ();
					/*
			if(ReturnNumberOfDeadPlayers() >= 9)
			{
				isWinner = true;
			}
*/
				
			break;
		default:
			break;
		};

		if (isWinner) {

			selfData.CmdAddNameToWinners ();
			
		} else if((int)Species.Crow != selfData.playerID && (int)Species.Snake != selfData.playerID && (int)Species.EgyptianPlover != selfData.playerID && (int)Species.Mouse != selfData.playerID && (int)Species.Hyena != selfData.playerID)
		{
			selfData.CmdAddNameToLosers ();
		}


	}

	public string getMyAnimalName(int ID)
	{
		return PlayerDataBank.instance.RequestPlayerAnimalName (ID);
	}

	public void DefendSelf(int AttackerID)
	{
		AudioManager.instance.PlayAtackSound ();

		if (AttackerID != selfData.playerID && selfData.isAlive) {
			
			print ("Started defending self");

			string attackerName = GetNameOfPlayerWithID(AttackerID);

			if (selfData.playerID == (int)Species.Snake || PlayerDataBank.instance.RequestPlayerLevel(selfData.playerID) >= PlayerDataBank.instance.RequestPlayerLevel(AttackerID)) {
				//nothing happens, you live.
				//broadcast locally
				if (selfData.playerID == (int)Species.Snake) {
					print ("local message: " + attackerName + " attacked you and died!");
					LocalText.text = "Your Encounters: " + attackerName + " attacked you and died!";
				} else 
				{
					print ("local message: " + attackerName + " attacked you and nothing happend");
					LocalText.text ="Your Encounters: " +  attackerName + " attacked you and nothing happend";
				}

			} 
			else if(selfData.playerID == (int)Species.Mallard || selfData.playerID == (int)Species.Rabit || selfData.playerID == (int)Species.Deer || selfData.playerID == (int)Species.Otter)
			{
				if (areAllSurvivorsCurrentlyInMyBubble ()) {
					// automatically cannot die
					print ("local message:" + attackerName + " attacked you and nothing happend.");
					LocalText.text ="Your Encounters: " +  attackerName + " attacked you and nothing happend.";
				} else if (PlayerDataBank.instance.RequestPlayerLevel (selfData.playerID) < PlayerDataBank.instance.RequestPlayerLevel (AttackerID)) {
					//I die
					//broadcast globally
					print (selfData.pName + " was killed by " + attackerName + "!");

					LocalText.text ="Your Encounters: " +  attackerName + " attacked you and you died";

					string message ="World Encounters: " + selfData.pName + " was killed by " + attackerName + "!";
					selfData.CmdupdateGlobalChat ( message);

					handleSelfDeath ();
				}
			}
			else if(PlayerDataBank.instance.RequestPlayerLevel(selfData.playerID) < PlayerDataBank.instance.RequestPlayerLevel(AttackerID))
			{
				// I die
				//Broadcast globally
				LocalText.text ="Your Encounters: " +  attackerName + " attacked you and you died";

				print ("Global message: " + selfData.pName + " was killed by " + attackerName + "!");
				string message ="World Encounters: " + selfData.pName + " was killed by " + attackerName + "!";
				selfData.CmdupdateGlobalChat ( message);
				handleSelfDeath ();

			}
		} 


	}

	public void comfirmAttack()
	{
		List<SetupLocalPlayer> nearbyPlayer = selfData.pList.returnNearbyPlayers ();

		if(LocalNearbyPlayers.options.Count == 0 && nearbyPlayer.Count == 0)
		{
			combatMenu.ToggleMenu ();
			return;
		}
		else if(LocalNearbyPlayers.options.Count == 0 && nearbyPlayer.Count > 0)
		{
			combatMenu.ToggleMenu ();
			combatMenu.ToggleMenu ();
			return;
		}

		for(int i = 0; i < nearbyPlayer.Count; i++)
		{
			if(i == LocalNearbyPlayers.value )
			{
				SetupLocalPlayer targetPlayer = nearbyPlayer [i];

				if(targetPlayer != null)
				{
					if (!targetPlayer.isAlive) {
						combatMenu.ToggleMenu ();
						combatMenu.ToggleMenu ();
						return;
					} else 
					{
						AttackOtherAnimal (targetPlayer);
						combatMenu.ToggleMenu ();
					}



				}
			
			}
		}	




	
	}

	
		
	public void AttackOtherAnimal(SetupLocalPlayer Opponent)
	{	
		AudioManager.instance.PlayAtackSound ();
		
		if(Opponent.playerID == selfData.playerID)
		{
			return;
		}
		//spawn attack object
		selfData.CmdSpawnObj(Opponent.gameObject.transform.position, Opponent.playerID);

		if (Opponent.playerID == (int)Species.Snake) {
			//You die
			//broadcast globally
			LocalText.text = "Your Encounters: You attacked " + Opponent.pName + " and died from their poison!";

			print ("Global message: " + selfData.pName + " was killed by " + Opponent.pName + "!");
			string message = "World Encounters: " + selfData.pName + " was killed by " + Opponent.pName + "!";
			selfData.CmdupdateGlobalChat (message);
			handleSelfDeath ();

		} else if (PlayerDataBank.instance.RequestPlayerLevel (selfData.playerID) <= PlayerDataBank.instance.RequestPlayerLevel (Opponent.playerID)) {
			//nothing happens	
			print ("local message: You attacked " + Opponent.pName + " and nothing happend.");
			LocalText.text = "Your Encounters: " + "You attacked " + Opponent.pName + " and nothing happend";
		

		} else if (Opponent.playerID == (int)Species.Mallard || Opponent.playerID == (int)Species.Rabit || Opponent.playerID == (int)Species.Deer || Opponent.playerID == (int)Species.Otter) {
			if (areAllSurvivorsCurrentlyInMyBubble ()) {
				// opponent automatically cannot die
				//nothing happens, broadcast Locally
				print ("local message: You attacked " + Opponent.pName + " and nothing happend.");
				LocalText.text = "Your Encounters: " + "You attacked " + Opponent.pName + " and nothing happend";



			} else if (PlayerDataBank.instance.RequestPlayerLevel (selfData.playerID) > PlayerDataBank.instance.RequestPlayerLevel (Opponent.playerID)) {
				//opponent dies
				//broadcast locally
				print ("local message: You killed " + Opponent.pName + "!");
				LocalText.text = "Your Encounters: " + "You killed " + Opponent.pName + "!";

				selfData.CmdSetAttackStatus (true);
				selfData.hasAttackedThisRound = true;



			}
		} else if (PlayerDataBank.instance.RequestPlayerLevel (selfData.playerID) > PlayerDataBank.instance.RequestPlayerLevel (Opponent.playerID)) {
			//I win
			//broadcast Locally

			print ("local message: You killed " + Opponent.pName + "!");
			LocalText.text = "Your Encounters: " + "You killed " + Opponent.pName + "!";

			selfData.hasAttackedThisRound = true;
			selfData.CmdSetAttackStatus (true);

		}
	

	}
	public void IncrementStarvationCounter()
	{
		selfData.starvationCount++;
		selfData.CmdSetStarvationCount (selfData.starvationCount);
	}
	public void ResetStarvationCounter()
	{
	//	selfData.starvationCount = 0;
	//	selfData.CmdSetStarvationCount (selfData.starvationCount);
	}

	public void checkForStarvation()
	{
		if(!selfData.isAlive)
		{
			return;
		}
		bool isStarved = false;


		switch(selfData.playerID)
		{
		case (int)Species.Crocodile:
			if(selfData.starvationCount >= 2)
			{
				isStarved = true;
			}
			break;
		case (int)Species.Eagle:
			if(selfData.starvationCount >= 2)
			{
				isStarved = true;
			}
			break;
		case (int)Species.Hyena:
			if(selfData.starvationCount >= 3)
			{
				isStarved = true;
			}
			break;
		case (int)Species.Lion:
			if(selfData.starvationCount >= 1)
			{
				isStarved = true;
			}
			break;
		case (int)Species.Mallard:
			if(TeleportHistory.Count == 3 && !TeleportHistory.Contains(2))
			{
				isStarved = true;
			}
			break;
		default:
			break;
		};

		if(isStarved)
		{
			handleSelfDeath ();
			DisplayStarvationText ();
			string message = "World Encounters: " + selfData.pName + " died of starvation!";
			selfData.CmdupdateGlobalChat (message);
		}


	}
	public void fillDropdownMenu()
	{
        PhotonPlayer[] players = PhotonNetwork.playerList;
        List<string> allPlayerNames = new List<string>();
        foreach (PhotonPlayer player in players)
        {
            if (player.NickName != photonView.owner.NickName)
            {
                allPlayerNames.Add(player.NickName);

            }
        }

        menuOne.AddOptions (allPlayerNames);
		CrowMenu.AddOptions (allPlayerNames);
	}
	
	public void setSelfData(SetupLocalPlayer myself)
	{
		selfData = myself;
	}
	public void addSelfData(SetupLocalPlayer myData)
	{
		//allNonSelfPlayers.Add (myData);
	}

	public int returnIDFromName(string nameToCheck)
	{
		int retval = 0;
        SetupLocalPlayer[] players = FindObjectsOfType<SetupLocalPlayer>();

        for (int i = 0; i < players.Length; i++)
		{
			if(players[i].pName == nameToCheck )
			{
				retval = players[i].playerID;
			}
		}
			

	
		return retval;

	}

	public void HostUpdateTime()
	{
		selfData.UpdateMins (mins.value);
		selfData.UpdateSecs (secs.value*10);


	}


	public void PeekPlayer()
	{
        /*
		if (orderedNameList.Count > 0 && EveryoneIsReadyForPeek ()) {
			string nameOfDesiredPlayer = orderedNameList [menuOne.value];
			int selectedPlayerID = 0;

			if (returnIDFromName (nameOfDesiredPlayer) == (int)Species.Chameleon) {

				selectedPlayerID = returnChameleonCamoID ();
				//selfData.localData.LoadPeekData (selectedPlayerID, returnChameleonName());

			} else {
				selectedPlayerID = returnIDFromName (nameOfDesiredPlayer);
				//selfData.localData.LoadPeekData (selectedPlayerID);

			}


		} else 
		{
			PeekNotificationText.Brighten ();
		}
        */

	}

	public bool EveryoneIsReadyForPeek()
	{
		bool ready = true;
        SetupLocalPlayer[] players = FindObjectsOfType<SetupLocalPlayer>();
		for(int i = 0; i < players.Length; i ++)
		{
			if(!players[i].readyForPeek)
			{
				ready = false;
			}
		}
		if(!selfData.readyForPeek)
		{
			ready = false;
		}

		return ready;
	}

	public string returnChameleonName()
	{
		//find the chameleon
		string retval = "";
        SetupLocalPlayer[] players = FindObjectsOfType<SetupLocalPlayer>();

        for (int i= 0; i < players.Length; i++)
		{
			if(players[i].playerID == (int)Species.Chameleon)
			{
				retval = players[i].pName;
			}
		}

		return retval;
	}

	public int returnChameleonCamoID()
	{
		//find the chameleon
		int retval = 0;
        SetupLocalPlayer[] players = FindObjectsOfType<SetupLocalPlayer>();

        for (int i= 0; i < players.Length; i++)
		{
			if(players[i].playerID == (int)Species.Chameleon)
			{
				retval = players[i].ChameleonCoverID;
			}
		}

		return retval;
	}
	public string GetNameOfPlayerWithID(int ID)
	{
		string temp = "";
        SetupLocalPlayer[] players = FindObjectsOfType<SetupLocalPlayer>();
		for(int i = 0; i < players.Length; i++)
		{
			if(players[i].playerID == ID)
			{
				temp = players[i].pName;
			}
		}

		return temp;

	}

	public void SetChameleonID()
	{

		selfData.IDChanged = true;
		
	}

	public void SetCrowWinner()
	{

		selfData.CrowIDChanged = true;
		
	}
	void reshuffle(List<string> names)

	{
		// Knuth shuffle algorithm :: courtesy of Wikipedia :)
		for (int t = 0; t < names.Count; t++ )
		{
			string tmp = names[t];
			int r = Random.Range(t, names.Count);
			names[t] = names[r];
			names[r] = tmp;
		}
	}



}
