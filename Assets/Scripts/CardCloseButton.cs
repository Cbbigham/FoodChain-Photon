using UnityEngine;
using System.Collections;

public class CardCloseButton : MonoBehaviour {

	public InfoCard AnimalInfo;
	public InfoCard PeekOneCard;
	public InfoCard PeekTwoCard;


	
	void Update () {
	
	}

	public void closeCard()
	{
		if(AnimalInfo.active)
		{
			AnimalInfo.toggleCardState ();
		}
		else if(PeekOneCard.active)
		{
			PeekOneCard.toggleCardState ();

		}
		else if(PeekTwoCard.active)
		{
			PeekTwoCard.toggleCardState ();

		}

	}
}
