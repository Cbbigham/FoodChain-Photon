using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PasswordCheck : MonoBehaviour {

	public InputField inputF;
	public string correctPassword;
	public GameObject MenuToLoad;
	public GameObject ThisMenu;
	public TextNotification notification;

	void Start () {
		
	}
	
	void Update () {
		
	}
	public void TryPassword()
	{
		if(PasswordIsCorrect())
		{
			MenuToLoad.SetActive (true);
			ThisMenu.SetActive (false);
		}
		else
		{
			notification.Brighten ();
		}
	}
	bool PasswordIsCorrect()
	{
		bool ret = false;

		if(inputF.text.ToLower() == correctPassword.ToLower())
		{
			ret = true;
		}

		return ret;
	}
}
