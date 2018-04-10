using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TimerObj : NetworkBehaviour {

	public int TimeLeft;
	public Text countdownText;
	LookAtTimer timer;

	public bool hasTimer = false;

	public void Start()
	{
		GameObject textObj = GameObject.Find ("TimeLeft");
		countdownText = textObj.GetComponent<Text> ();
		TimeLeft = 0;
	}
	
	void Update () {
	
	}

	int getMins(int time)
	{
		int ret = 0;

		ret = Mathf.FloorToInt (time / 60);

		return ret;
	}

	int getSecs(int time)
	{
		int ret = 0;

		ret = time % 60;


		return ret;
	}
	[ClientRpc]
	public void RpcPostSetupTick()
	{
		if(timer == null)
		{
			timer = FindObjectOfType<LookAtTimer> ();
		}

		timer.PostSetupRoundCheck ();
	}

	[ClientRpc]
	public void RpcPostStrategyTick()
	{
		if(timer == null)
		{
			timer = FindObjectOfType<LookAtTimer> ();
		}
		timer.PostStrategyRoundCheck ();
	}
	[ClientRpc]
	public void RpcCombatTick()
	{
		if(timer == null)
		{
			timer = FindObjectOfType<LookAtTimer> ();
		}

		timer.postCombatCheck ();
	}


	[ClientRpc]
	public void RpcUpdateCountdown(int countdown)
	{
		if(timer == null)
		{
			timer = FindObjectOfType<LookAtTimer> ();
		}

		TimeLeft = countdown;


		timer.numSecs = countdown;
		timer.Tick ();

		if(countdownText == null)
		{
			countdownText = FindObjectOfType<MyTime>().gameObject.GetComponent <Text> ();
		}


		if (getSecs (countdown) < 10) {

			countdownText.text = getMins (countdown).ToString () + ":0" + getSecs (countdown).ToString ();

		} else 
		{
			countdownText.text = getMins (countdown).ToString () + ":" + getSecs (countdown).ToString ();

		}
	}

}
