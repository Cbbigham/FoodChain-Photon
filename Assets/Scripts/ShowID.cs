using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
public class ShowID : NetworkBehaviour {

	public string previousText;

	void OnEnable()
	{
		
			GetComponent<Text> ().text = previousText + "(" + Network.player.ipAddress +")";
		

	}


}
