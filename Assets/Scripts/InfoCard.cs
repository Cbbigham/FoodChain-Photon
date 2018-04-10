using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class InfoCard : MonoBehaviour {

	public Text Cardname;
	public Text special;
	public Text win;
	public Text lose;
	public Text environment;


	public bool active;
	public GameObject card;

	public GameObject cardOne;
	public GameObject cardTwo;

	public PeekMenuToggle cover;

	public void SetCard(string _name, string _spec, string _win, string _lose, string _environment, Sprite Icon)
	{	
		active = false;
		Cardname.text = "Animal: " + _name;
		special.text = "Special: " + _spec;
		win.text = "Win: " + _win;
		lose.text = "Lose: " + _lose;
		environment.text = "Environment: " + _environment;

	}

	public void toggleCardState()
	{
		AudioManager.instance.PlayButtonSound ();
		active = !active;

		if (active) {


			cover.ToggleMenu ();
			
				
			card.SetActive (true);
		} else 
		{
			cover.ToggleMenu ();

			
			card.SetActive (false);
		}
	}
}
