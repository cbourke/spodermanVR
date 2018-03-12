using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1_1EndHandle : MonoBehaviour {

	public GameObject eventHandler;
	public GameObject leftController;
	public GameObject rightController;

	public void OnTriggerEnter(Collider other) {
		if (other.gameObject.Equals(leftController) || other.gameObject.Equals(rightController)) {
			eventHandler.GetComponent<LevelEvents1_1> ().showMessage ();
		}
	}
}
