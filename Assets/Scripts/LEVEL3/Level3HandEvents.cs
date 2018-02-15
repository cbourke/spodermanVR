using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3HandEvents : MonoBehaviour {

	public GameObject key;
	public GameObject carButton;
	public Level3Events world;
	public EventUtil util;
	// Use this for initialization
	void Awake () {
		world = GameObject.Find ("WorldNodeTracker").transform.Find("Events").GetComponent<Level3Events>();
		util = GameObject.Find ("WorldNodeTracker").transform.Find("Events").GetComponent<EventUtil> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (this.GetComponent<ControllerGrab> ().objectInHand != null) {
			Debug.Log (this.GetComponent<ControllerGrab> ().objectInHand.GetInstanceID () );
			Debug.Log (carButton.GetInstanceID ());
		}

		if (world.keyObtained && this.GetComponent<ControllerGrab> ().objectInHand != null && this.GetComponent<ControllerGrab> ().objectInHand.GetInstanceID () == carButton.GetInstanceID ()) {
			world.truckMoving = true;
		} else
			world.truckMoving = false;
	}



	public void OnTriggerEnter (Collider coll) {
		if (coll.gameObject.GetInstanceID() == key.GetInstanceID()) {
			world.GetComponent<Level3Events> ().keyObtained = true;
			Destroy (key);
			util.playClip (this.gameObject , (AudioClip)Resources.Load("Audio/General/softCorrectSound"));
		}

//		if (coll.gameObject.GetInstanceID() == carButton.GetInstanceID() && world.keyObtained) {
//
//		}
	}
}
