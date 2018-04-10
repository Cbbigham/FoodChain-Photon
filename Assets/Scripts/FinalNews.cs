using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
public class FinalNews : NetworkBehaviour {

	NewsPanel panel;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	[ClientRpc (channel = 1)]
	public void RpcDisplayNews(string winners, string losers)
	{
		if(panel == null)
		{
			panel = FindObjectOfType<NewsReference> ().reference;
			FindObjectOfType<NewsReference> ().menu.ToggleMenu ();
		}

		panel.text.text = "Winners: " + winners + ".  Losers: " + losers + ".";
		
	}
}
