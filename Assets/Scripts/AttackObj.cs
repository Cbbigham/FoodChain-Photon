using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class AttackObj : NetworkBehaviour {

	[SyncVar]
	public int attackerID;
	[SyncVar]
	public int TargetID;

	void Start () {
	
	}

	void OnCollisionEnter(Collision c)
	{
		gameObject.SetActive (false);
	}


	[ClientRpc]
	public void RpcAttack()
	{
		

		if(FindObjectOfType<LocalServerInfo>().selfData.playerID == TargetID)
		{
			
			FindObjectOfType<LocalServerInfo>().DefendSelf (attackerID);


		}
	}



		

	void Update () {
	
	}
}
