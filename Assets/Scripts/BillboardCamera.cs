using UnityEngine;
using System.Collections;

public class BillboardCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		transform.LookAt (Camera.main.transform.position);
		transform.Rotate (new Vector3 (0, 180, 0));
	}
}
