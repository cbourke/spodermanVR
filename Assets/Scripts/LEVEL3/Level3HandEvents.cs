using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3HandEvents : MonoBehaviour {

	public GameObject key;
	public GameObject carButton;
	public Level3Events world;
	public EventUtil util;
	private ControllerGrab otherController;
	// Use this for initialization
	void Awake () {
		world = GameObject.Find ("WorldNodeTracker").transform.Find("Events").GetComponent<Level3Events>();
		util = GameObject.Find ("WorldNodeTracker").transform.Find("Events").GetComponent<EventUtil> ();
		if (this.name.Equals("Controller (left)")) 
			otherController = GameObject.Find ("[CameraRig]").transform.Find ("Controller (right)").gameObject.GetComponent<ControllerGrab>();
		else 
			otherController = GameObject.Find ("[CameraRig]").transform.Find ("Controller (left)").gameObject.GetComponent<ControllerGrab>();
	}
	
	// Update is called once per frame
	void Update () {

		if (!world.keyObtained && !world.window2.activeSelf && (util.ObjectInHandCheckLeft().GetInstanceID() == carButton.GetInstanceID() || util.ObjectInHandCheckRight().GetInstanceID() == carButton.GetInstanceID() )) {
			world.window2.SetActive (true);
		}

		if (world.keyObtained && this.GetComponent<ControllerGrab> ().objectInHand != null && this.GetComponent<ControllerGrab> ().objectInHand.GetInstanceID () == carButton.GetInstanceID ()) {
			if (this.gameObject.name.Equals ("Controller (left)")) {
				world.truckMovingL = true;
			} else {
				world.truckMovingR = true;
			}
		} else {
			if (this.gameObject.name.Equals ("Controller (left)")) {
				world.truckMovingL = false;
			} else {
				world.truckMovingR = false;
			}
		}
	}



	public void OnTriggerEnter (Collider coll) {
		if (coll.gameObject.GetInstanceID() == key.GetInstanceID()) {
			world.GetComponent<Level3Events> ().keyObtained = true;
			Destroy (key);
			util.playClip (this.gameObject , (AudioClip)Resources.Load("Audio/General/softCorrectSound"));
			util.GetAnimFromWindow (world.window2).SetTrigger ("TurnOff");
		}

//		if (coll.gameObject.GetInstanceID() == carButton.GetInstanceID() && world.keyObtained) {
//
//		}
	}
}
