using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    public LocalPlayerData _target;
    public string PlayerName;


    public bool isOctopus;
    public bool isTurtle;

    public int numPeeksLeft;

    public GameObject PeekButton;

    public InfoCard cardOne;
    public InfoCard cardTwo;

    public GameObject PeekOneRefButton;
    public GameObject PeekTwoRefButton;

    public GameObject octopusButton;
    public GameObject turtleButton;

    public Text PeekOneText;
    public Text PeekTwoText;
    PlayerDataBank dataBank;
    public InfoCard localCard;

   public bool hasTwoPeeks;

    private void Awake()
    {
        this.GetComponent<Transform>().SetParent(GameObject.Find("Canvas").GetComponent<Transform>());
        GetComponent<Transform>().localPosition = Vector3.zero;
        GetComponent<Transform>().localScale = Vector3.one;
        GetComponent<RectTransform>().sizeDelta = Vector2.zero;

    }
    public void SetTarget(LocalPlayerData target)
    {
        if (target == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
            return;
        }
        // Cache references for efficiency
        _target = target;
      
            PlayerName = _target.photonView.owner.NickName;
        
    }
    // Update is called once per frame
    void Update () {
        // Destroy itself if the target is null, It's a fail safe when Photon is destroying Instances of a Player over the network
        if (_target == null)
        {
            Destroy(this.gameObject);
            return;
        }
    }
}
