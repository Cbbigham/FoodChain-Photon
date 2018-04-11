using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMaster : Photon.MonoBehaviour
{

    public static PlayerMaster instance;
    public bool isGameReady;
    public List<DoozyUI.NavigationPointer> MasterMenu; 
	void Awake () {
        instance = this;
        isGameReady = false;
	}
	
	void Update () {
		
	}

    public void CheckForRoomFill()
    {
        if (PhotonNetwork.playerList.Length >= 2)
        {
            Debug.Log("Game Ready!");
            isGameReady = true;
            MasterMenu = new List<DoozyUI.NavigationPointer>();
            MasterMenu.Add(new DoozyUI.NavigationPointer("UI", "MasterClientControl"));
            DoozyUI.UINavigation.Show(MasterMenu);
          
            photonView.RPC("DisableWaitForPlayersMenu", PhotonTargets.All);

        }
    }

    void AssignAnimals()
    {

    }
}
