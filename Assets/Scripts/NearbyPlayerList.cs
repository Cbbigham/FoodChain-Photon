using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NearbyPlayerList : MonoBehaviour {


	public List<SetupLocalPlayer> nearbyPlayers;




	void OnTriggerStay(Collider c)
	{
		
		if(c.gameObject.GetComponent<Grave>())
		{
			Grave g = c.gameObject.GetComponent<Grave> ();
		
			nearbyPlayers.Clear ();

			Destroy (g);
		}
	
		if(c.gameObject.GetComponent<SetupLocalPlayer>())
		{
			
			SetupLocalPlayer setup = c.gameObject.GetComponent<SetupLocalPlayer> ();

			if(!nearbyPlayers.Contains(setup))
			{
				nearbyPlayers.Add (setup);
			}
			
		}
	
	}

	public List<SetupLocalPlayer> returnNearbyPlayers()
	{
		return nearbyPlayers;
	}
	void OnTriggerExit(Collider c)
	{
		

		if(c.gameObject.GetComponent<SetupLocalPlayer>())
		{

			SetupLocalPlayer setup = c.gameObject.GetComponent<SetupLocalPlayer> ();

			if(nearbyPlayers.Contains(setup))
			{
				nearbyPlayers.Remove(setup);
			}

		}

	}


}
