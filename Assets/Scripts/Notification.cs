using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class Notification : MonoBehaviour {

	Image i;

	void Start () {

		i = GetComponent<Image>();


	}

	public void Brighten()
	{
		i = GetComponent<Image>();

		i.color = new Color (i.color.r, i.color.g, i.color.b, 1);

	

	}

	// Update is called once per frame
	void Update () {

		if(i.color.a > 0)
		{
			i.color = new Color (i.color.r, i.color.g, i.color.b, i.color.a - 0.01f);
		}
		if(i.color.a < 0)
		{
			i.color = new Color (i.color.r, i.color.g, i.color.b, 0);
		}



	}
}
