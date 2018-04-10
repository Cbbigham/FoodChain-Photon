using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class TextNotification : MonoBehaviour {

	Text t;

	void Start () {

		t = GetComponent<Text>();


	}

	public void Brighten()
	{
		t = GetComponent<Text>();

		t.color = new Color (t.color.r, t.color.g, t.color.b, 1);



	}

	// Update is called once per frame
	void Update () {

		if(t.color.a > 0)
		{
			t.color = new Color (t.color.r, t.color.g, t.color.b, t.color.a - 0.004f);
		}
		if(t.color.a < 0)
		{
			t.color = new Color (t.color.r, t.color.g, t.color.b, 0);
		}



	}
}
