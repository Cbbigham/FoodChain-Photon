using UnityEngine;
using System.Collections.Generic;

using System.Collections;
using UnityEngine.UI;

public class Player : Photon.PunBehaviour, IPunObservable
{


   // public PhotonView photonView;

    public string pName;
    public TextMesh PlayerName;
    
    void Start()
    {
        PhotonNetwork.sendRate = 20;
        PhotonNetwork.sendRateOnSerialize = 10;
        pName = PhotonNetwork.playerName;
        PlayerName.text = pName;
        if (PhotonNetwork.otherPlayers.Length == 0)
        {
            gameObject.AddComponent<PlayerMaster>();
        }
       // photonView = GetComponent<PhotonView>();
      //  photonView.owner.name = PhotonNetwork.playerName;
    }
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {

            stream.SendNext(pName);
      


        }
        else if (stream.isReading)
        {
            pName = (string)stream.ReceiveNext();
       
         

        }

        PlayerName.text = pName;

    }

    [PunRPC]
    public void DisableWaitForPlayersMenu()
    {
        List<DoozyUI.NavigationPointer> ps = new List<DoozyUI.NavigationPointer>();
        ps.Add(new DoozyUI.NavigationPointer("UI","WaitingForPlayers"));
        DoozyUI.UINavigation.Hide(ps);
    }
   

  

}
