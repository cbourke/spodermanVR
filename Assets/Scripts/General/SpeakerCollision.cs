using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakerCollision : MonoBehaviour {

	public void OnTriggerEnter(Collider other) {

		Destroy (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
