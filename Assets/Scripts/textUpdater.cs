using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
public class textUpdater : NetworkBehaviour {

	public Text GlobalText;


	void Start () {
	
	}
	
	void Update () {
	
	}
	[ClientRpc (channel = 1)]
	public void RpcUpdateGlobalText(string newText)
	{
		
		GlobalText = GameObject.Find ("GlobalText").GetComponent<Text>();
		GlobalText.text = newText;
		AudioManager.instance.PlayDeathSound ();
	}

}
