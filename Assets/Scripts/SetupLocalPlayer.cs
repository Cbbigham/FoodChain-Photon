using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SetupLocalPlayer : Photon.PunBehaviour, IPunObservable {



	public enum Animal
	{
		Chameleon = 0, Crocodile = 1, Crow = 2, Deer = 3, Eagle = 4, EgyptianPlover = 5,
		Hyena = 6, Lion = 7, Mallard = 8, Mouse = 9, Otter = 10, Rabit = 11, Snake = 12 

	};

    public PhotonView photonView;

    public string pName;
    public TextMesh PlayerName;
    public Color playerColor = Color.white;

	public int playerID;

	public bool readyForPeek;

	public int ChameleonCoverID;

	public int CrowWinnerGuess;

	public bool donePeeking;

	public bool isAlive;

	public int starvationCount;

	public bool hasAttackedThisRound;



	public LocalPlayerData localData;
	public LocalServerInfo serverDataBank;

	public NearbyPlayerList pList;

	public bool IDChanged;
	public bool CrowIDChanged;

	public GameObject AttackObj;
	public GameObject grave;

    void Start()
    {
        PhotonNetwork.sendRate = 20;
        PhotonNetwork.sendRateOnSerialize = 10;

    }
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {

            stream.SendNext(pName);
          //  stream.SendNext(playerColor);
            stream.SendNext(playerID);
            stream.SendNext(readyForPeek);
            stream.SendNext(ChameleonCoverID);
            stream.SendNext(CrowWinnerGuess);
            stream.SendNext(donePeeking);
            stream.SendNext(isAlive);
            stream.SendNext(starvationCount);
            stream.SendNext(hasAttackedThisRound);


        }
        else if (stream.isReading)
        {
            pName = (string)stream.ReceiveNext();
            //playerColor = (Color)stream.ReceiveNext();
            playerID = (int)stream.ReceiveNext();
            readyForPeek = (bool)stream.ReceiveNext();
            ChameleonCoverID = (int)stream.ReceiveNext();
            CrowWinnerGuess = (int)stream.ReceiveNext();
            donePeeking = (bool)stream.ReceiveNext();
            isAlive = (bool)stream.ReceiveNext();
            starvationCount = (int)stream.ReceiveNext();
            hasAttackedThisRound = (bool)stream.ReceiveNext();

        }

        PlayerName.text = photonView.owner.NickName;

    }
    public void Init(string playerName) {

		IDChanged = false;
		CrowIDChanged = false;
        pName = playerName;
		serverDataBank = FindObjectOfType<LocalServerInfo> ();


		Renderer[] rends = GetComponentsInChildren<Renderer> ();

		foreach(Renderer r in rends)
		{
			r.material.color = playerColor;

		}

	

		if (photonView.isMine) {
			serverDataBank.setSelfData (this);

			InitializeThisCharacterLocally ();
			pList = GetComponentInChildren<NearbyPlayerList> ();
			CmdSetAliveStatus (true);
			CmdSetStarvationCount (0);
			CmdSetAttackStatus (false);
            
		    CmdPushName ();

		
    

		} else 
		{
			//serverDataBank.addSelf (pName);
			serverDataBank.addSelfData (this);
		}
	
	}


	public void CmdPushNameServer()
	{
		if(playerID == (int)Animal.Snake)
		{
			serverDataBank.winers.snakeName = (pName);
		}
		else if(playerID == (int)Animal.Hyena)
		{
			serverDataBank.winers.hyenaName = (pName);

		}
		else if(playerID == (int)Animal.EgyptianPlover)
		{
			serverDataBank.winers.PloverName = (pName);

		}
		else if(playerID == (int)Animal.Mouse)
		{
			serverDataBank.winers.mouseName =  (pName);

		}
	}

	public void CmdPushName()
	{
		if(playerID == (int)Animal.Snake)
		{
			serverDataBank.winers.CmdSetSnakeName (pName);
		}
		else if(playerID == (int)Animal.Hyena)
		{
			serverDataBank.winers.CmdSetHyenaName (pName);

		}
		else if(playerID == (int)Animal.EgyptianPlover)
		{
			serverDataBank.winers.CmdPloverName (pName);

		}
		else if(playerID == (int)Animal.Mouse)
		{
			serverDataBank.winers.CmdMouseName (pName);

		}
	}

	public void CmdCheckSnakeWin()
	{
		serverDataBank.winers.CmdCheckSnakeWin ();
	}

	public void MoveSelfToRandomPointInEnvironment(Vector3 Center)
	{
		if(!isAlive)
		{
			return;
		}
		
			pList.nearbyPlayers.Clear ();
			float deviation = 7.0f;

			Vector3 randomPos = new Vector3 (Center.x + Random.Range(-deviation,deviation), Center.y, Center.z + Random.Range(-deviation,deviation));
			transform.position = randomPos;	

			CmdSetAttackStatus (false);



	}


	void Update()
	{
		if(!photonView.isMine)
		{
			return;
		}

		if(!isAlive && Time.timeSinceLevelLoad > 10)
		{
			transform.position = Vector3.Lerp (transform.position, new Vector3(transform.position.x, -300, transform.position.z),Time.deltaTime );
		}

		if(CrowIDChanged)
		{
			CrowIDChanged = false;


			string animal;
			int ID = 0;

			if (serverDataBank.CrowMenu.value < 2) {
				animal = PlayerDataBank.instance.PlayerInfoList [serverDataBank.CrowMenu.value].Name;
				ID = serverDataBank.CrowMenu.value;

			} else 
			{
				animal = PlayerDataBank.instance.PlayerInfoList [serverDataBank.CrowMenu.value + 1].Name;
				ID = serverDataBank.CrowMenu.value + 1;

			}
				
			CmdSetCrowGuess (serverDataBank.CrowMenu.value);
			CmdSumbitCrowGuess (animal, ID);
		}
		else if(IDChanged)
		{
			CmdSetChameleonID (serverDataBank.ChameleonMenu.value + 1);
			IDChanged = false;
		}
	}
	
	public void setChameleonId(int ID)
	{
		CmdSetChameleonID (ID);
	}

    public void CmdAddNameToWinners()
	{
		serverDataBank.winers.CmdAddNameToWinners (pName , serverDataBank.getMyAnimalName(playerID), playerID);
	}


    public void CmdAddNameToLosers()
	{
		serverDataBank.winers.CmdAddNameToLosers (pName , serverDataBank.getMyAnimalName(playerID), playerID);

	}
	public void CmdSetAttackStatus(bool status)
	{
		hasAttackedThisRound = status;
	}
	public void CmdSetStarvationCount(int count)
	{
		starvationCount = count;
	}

	public void CmdupdateGlobalChat(string updateText)
	{
		serverDataBank.chatManager.CmdUpdateGlobalText (updateText);
	}

	public void CmdCreateGrave()
	{
		GameObject g = (GameObject)Instantiate (grave, transform.position, Quaternion.identity);
		//NetworkServer.Spawn (g);
	}

	public void CmdSpawnObj(Vector3 pos, int TargetID)
	{
//		SetupLocalPlayer attacker = serverDataBank.selfData;
		Vector3 displacement = new Vector3 (0,4,0);
		GameObject attackObject = (GameObject)Instantiate (AttackObj, pos + displacement, Quaternion.identity);
		AttackObj reference = attackObject.GetComponent<AttackObj> ();
		reference.attackerID = (playerID);
		reference.TargetID =  (TargetID);
		//NetworkServer.Spawn (attackObject);
		reference.RpcAttack ();

	}

	public void CmdSetAliveStatus(bool alive)
	{
		isAlive = alive;

		if(alive == false)
		{
			serverDataBank.winers.CmdincrementDeathCount ();
		}
	}
	
	public void CmdSetPeekDoneStatus(bool status)
	{
		donePeeking = status;
	}

	public void CmdSetChameleonID(int ID)
	{
		ChameleonCoverID = ID;
		CmdChangePeekStatus (true);
	//	localData.DisableChameleonButton ();
	}
	public void CmdSetCrowGuess(int guess)
	{
		CrowWinnerGuess = guess;
		CmdChangePeekStatus (true);
	//	localData.DisableCrowButton ();
	}

	public void CmdSumbitCrowGuess(string animal, int ID)
	{
		serverDataBank.winers.CmdSetCrowGues (animal, ID);
		serverDataBank.winers.CmdSetCrowName (pName);
	}

	public void InitializeThisCharacterLocally()
	{

		CmdChangePeekStatus (true);
		CmdSetPeekDoneStatus (false);

		//determine local peek value
		if (playerID == (int)Animal.EgyptianPlover || playerID == (int)Animal.Crow || playerID == (int)Animal.Mouse) {

		//	localData.SetPeeks (2);
		} else 
		{
			//localData.SetPeeks (1);

		}

		if (playerID == (int)Animal.Chameleon) {

		//	localData.isCameleon = true;
			CmdChangePeekStatus (false);
		} else 
		{
			//localData.isCameleon = false;
		}

		if (playerID == (int)Animal.Crow) {
		//	localData.isCrow = true;
			CmdChangePeekStatus (false);


		} else 
		{
			//localData.isCrow = false;
		}


	//	localData.SetupLocalConnections ();
		//localData.setupLocalData ();
	

	}

	public void UpdateMins(int mins)
	{
		ServerTimer t = FindObjectOfType<ServerTimer> ();

		t.CmdSetMins (mins);
	}
	public void UpdateSecs(int secs)
	{
		ServerTimer t = FindObjectOfType<ServerTimer> ();

		t.CmdSetSecs (secs);
	}
	public void defendSelf(int OpponentID)
	{
		
		serverDataBank.DefendSelf (OpponentID);
	}

	public void CmdChangeName(string name)
	{
		pName = name;
	}
	public void CmdChangeID(int id)
	{
		playerID = id;
	} 
	public void CmdChangePeekStatus(bool status)
	{
		readyForPeek = status;


	} 

}
