using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
public class RoundNameUpdate : NetworkBehaviour {


	public Text roundTitle;
	public GameObject combatButton;
	public GameObject moveButton;
	public LocalServerInfo serverInfo;

	void Start () {
	
	}
	
	void Update () {
	
	}

	[ClientRpc()]
	public void RpcSetRoundStrategyTitle(int round)
	{
		if(roundTitle == null)
		{
			roundTitle = FindObjectOfType<RoundTitle>().gameObject.GetComponent <Text> ();

		}

		roundTitle.text = "Strategy Round " + round.ToString ();
	}

	[ClientRpc]
	public void RpcSetRoundCombatTitle(int round)
	{
		if(roundTitle == null)
		{
			roundTitle = FindObjectOfType<RoundTitle>().gameObject.GetComponent <Text> ();

		}

		roundTitle.text = "Combat Round " + round.ToString();
	}

	[ClientRpc]
	public void RpcSetRoundGameOverTitle()
	{
		if(roundTitle == null)
		{
			roundTitle = FindObjectOfType<RoundTitle>().gameObject.GetComponent <Text> ();

		}

		roundTitle.text = "Game Over";
	}

	[ClientRpc]
	public void RpcActivateMoveButton()
	{
		if(moveButton == null)
		{
			moveButton = FindObjectOfType<ButtonPanel> ().environmentButton;
		}

		moveButton.SetActive (true);
	}
	[ClientRpc]
	public void RpcCallGameOverCheck()
	{
		if(serverInfo == null)
		{
			serverInfo = FindObjectOfType<LocalServerInfo> ();
		}

		serverInfo.checkPlayerForWin ();
	
	}


	[ClientRpc]
	public void RpcDeactivateMoveButton()
	{
		if(moveButton == null)
		{
			moveButton = FindObjectOfType<ButtonPanel> ().environmentButton;

		}
		if(serverInfo == null)
		{
			serverInfo = FindObjectOfType<LocalServerInfo> ();
		}
			
		moveButton.SetActive (false);
	}



}
