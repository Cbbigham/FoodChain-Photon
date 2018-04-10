using UnityEngine;
using System.Collections;

public class ParentPanel : MonoBehaviour {

	public GameObject child;

	public void activateChildObj()
	{
		child.SetActive (true);
	}
}
