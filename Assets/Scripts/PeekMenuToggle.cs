using UnityEngine;
using System.Collections;

public class PeekMenuToggle : MonoBehaviour {

	public bool active;

	public GameObject closeButton;
	public bool isCover;
	void Start()
	{

	}
	public void SetTrue()
	{
		active = true;
	}
	public void ToggleMenu()
	{
		AudioManager.instance.PlayButtonSound ();

		active = !active;

		if (active) {
			if(isCover)
			{
				closeButton.SetActive (true);
			}
			gameObject.SetActive (true);

		} else 
		{
			if(isCover)
			{
				closeButton.SetActive (false);
			}
			gameObject.SetActive (false);

		}
	}
}