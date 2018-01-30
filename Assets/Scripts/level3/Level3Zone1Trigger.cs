using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Zone1Trigger : MonoBehaviour {


	private GameObject playerHead;
	private GameObject events;
	private bool triggered;
	// Use this for initialization
	void Awake () {
		playerHead = GameObject.Find ("CameraZoneCollider");
		events = GameObject.Find ("Events");
		triggered = false;
	}

	public void OnTriggerEnter (Collider coll) {
		if (coll.gameObject.GetInstanceID() == playerHead.GetInstanceID() && !triggered) {
			triggered = true;
			StartCoroutine(events.GetComponent<LevelEvents3> ().triggerZone1 ());
		}
	}
}
