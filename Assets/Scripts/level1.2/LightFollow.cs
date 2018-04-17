using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFollow : MonoBehaviour {

	public GameObject papa;
	
	// Update is called once per frame
	void Update () {
		this.transform.position = new Vector3 (papa.transform.position.x , papa.transform.position.y + 8.0f , papa.transform.position.z + 2.39f);
		transform.LookAt(papa.transform.position);
	}
}
