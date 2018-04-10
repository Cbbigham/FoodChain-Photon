using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.Networking;

public class ServerLivingStatus : NetworkBehaviour {

	public List<bool> lifeStatus;

	void Start () {
	
	}
	
	void Update () {
	
	}	 

	[Command]
	public void CmdMarkAsDead(int ID)
	{
		lifeStatus [ID] = false;	
	}
}
