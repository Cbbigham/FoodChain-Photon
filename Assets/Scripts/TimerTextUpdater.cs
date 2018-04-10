using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TimerTextUpdater : NetworkBehaviour {

	public Text countdownText;

	void Start () {
	
	}
	
	void Update () {
	
	}

	[ClientRpc]
	public void RpcUpdateClientTimerText(int mins, int secs)
	{
		if(countdownText == null)
		{
			countdownText = FindObjectOfType<MyTime>().gameObject.GetComponent <Text> ();
		}
	

		if (secs < 10) {

			countdownText.text = mins.ToString () + ":0" + secs.ToString ();

		} else 
		{
			countdownText.text = mins.ToString () + ":" + secs.ToString ();


		}
	}
}
