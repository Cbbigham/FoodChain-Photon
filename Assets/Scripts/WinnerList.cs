using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class WinnerList : NetworkBehaviour {
	
	public List<string> winners;
	public List<string> losers;

	public string crowGuess;
	public string crowName;


	public string snakeName;
	public string hyenaName;
	public string mouseName;
	public string PloverName;

	public int numTotalExpectedSumbission;

	public GameObject endGameNews;
	public FinalNews reference;

	public int deathCount;

	public int CrowGuessID;

	public enum Species
	{
		Chameleon = 0, Crocodile = 1, Crow = 2, Deer = 3, Eagle = 4, EgyptianPlover = 5,
		Hyena = 6, Lion = 7, Mallard = 8, Mouse = 9, Otter = 10, Rabit = 11, Snake = 12 

	};
	// Use this for initialization
	void Start () {
		CrowGuessID = 0;
		deathCount = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	[Command (channel = 1)]
	public void CmdAddNameToWinners(string name, string animal, int ID)
	{
		
		winners.Add (name + " (" +animal+ ")");

		numTotalExpectedSumbission = FindObjectsOfType<SetupLocalPlayer> ().Length;

		if(ID == CrowGuessID)
		{
			winners.Add (crowName +  " (Crow)");
		}
		if(ID == (int)Species.Lion)
		{
			losers.Add(hyenaName + " (Hyena)");	
			winners.Add (mouseName + " (Mouse)");
		}
		if(ID == (int)Species.Crocodile)
		{
			winners.Add(PloverName + " (Egyptian Plover)");
		}

		if((winners.Count + losers.Count) == numTotalExpectedSumbission)
		{
			
			GameObject news = (GameObject)Instantiate (endGameNews, Vector3.zero, Quaternion.identity);
			reference = news.GetComponent<FinalNews> ();
			
			string win = "";
			string lose = "";


			for(int i = 0; i < winners.Count; i++)
			{
				if (i < winners.Count - 1) {

					win += winners [i] + ", ";

				} else 
				{
					win += winners [i];

				}
			}

			for(int i = 0; i < losers.Count; i++)
			{
				if (i < losers.Count - 1) {

					lose += losers [i] + ", ";

				} else 
				{
					lose += losers [i];

				}
			}

			NetworkServer.Spawn (news);
			reference.RpcDisplayNews (win, lose);

		}
	}

	[Command (channel = 1)]
	public void CmdAddNameToLosers(string name, string animal, int ID)
	{
		
		losers.Add (name + " (" +animal+")");

		numTotalExpectedSumbission = FindObjectsOfType<SetupLocalPlayer> ().Length;

		if(ID == CrowGuessID)
		{
			losers.Add (crowName + " (Crow)");
		}
		if(ID == (int)Species.Lion)
		{
			winners.Add(hyenaName + " (Hyena)");	
			losers.Add (mouseName + " (Mouse)");
		}
		if(ID == (int)Species.Crocodile)
		{
			losers.Add(PloverName + " (Egyptian Plover)");
		}

		if((winners.Count + losers.Count) == numTotalExpectedSumbission)
		{
			GameObject news = (GameObject)Instantiate (endGameNews, Vector3.zero, Quaternion.identity);
			reference = news.GetComponent<FinalNews> ();

			string win = "";
			string lose = "";
	
			for(int i = 0; i < winners.Count; i++)
			{
				if (i < winners.Count - 1) {

					win += winners [i] + ", ";

				} else 
				{
					win += winners [i];

				}
			}

			for(int i = 0; i < losers.Count; i++)
			{
				if (i < losers.Count - 1) {

					lose += losers [i] + ", ";

				} else 
				{
					lose += losers [i];

				}
			}

			NetworkServer.Spawn (news);
			reference.RpcDisplayNews (win, lose);
		}

	}

	[Command]
	public void CmdincrementDeathCount()
	{
		deathCount++;
	}
	[Command]
	public void CmdCheckSnakeWin()
	{
		if (deathCount >= 9) {
			winners.Add (snakeName + " (Snake)");
		} else 
		{
			losers.Add (snakeName + " (Snake)");

		}
	}

	[Command (channel = 1)]
	public void CmdSetCrowGues(string guess, int ID)
	{
		crowGuess = guess;
		CrowGuessID = ID;
	}
	[Command (channel = 1)]
	public void CmdSetCrowName(string name)
	{
		crowName = name;
	}
	public void CmdSetHyenaName(string name)
	{
		hyenaName = name;
	}
	public void CmdMouseName(string name)
	{
		mouseName = name;
	}
		public void CmdPloverName(string name)
	{
	
		PloverName = name;
	}

	public void CmdSetSnakeName(string name)
	{
		snakeName = name;
	}

}
