using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GlobalChatManager : NetworkBehaviour {

	public Text globalText;
	public GameObject textChanger;
	public textUpdater reference;
	// Use this for initialization
	void Start () {
	
	}

	[Command]
	public void CmdUpdateGlobalText(string updateText)
	{
		
			GameObject newText = (GameObject)Instantiate (textChanger);
			reference = newText.GetComponent<textUpdater> ();
			NetworkServer.Spawn (newText);


		reference.RpcUpdateGlobalText (updateText);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
