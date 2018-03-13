using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3ZoneTriggers : MonoBehaviour {

	public GameObject colliding;
	public GameObject playerHead;
	private GameObject events;
	private bool triggered1;
	private bool triggered2;
	private bool triggered3;
	// Use this for initialization
	void Awake () {
		playerHead = GameObject.Find ("CameraZoneCollider");
		events = GameObject.Find ("Events");
		triggered1 = false;
		triggered2 = false;
		triggered3 = false;
	}

	public void OnTriggerEnter (Collider coll) {
		colliding = coll.attachedRigidbody.gameObject;
		if (coll.gameObject.GetInstanceID() == playerHead.GetInstanceID() && !triggered1 && this.name.Equals("Zone1")) {
			triggered1 = true;
			StartCoroutine(events.GetComponent<LevelEvents3> ().triggerZone1 ());
		}

		if (coll.gameObject.GetInstanceID() == playerHead.GetInstanceID() && !triggered2 && this.name.Equals("Zone2")) {
			triggered2 = true;
			events.GetComponent<LevelEvents3> ().triggerZone2 ();
		}

		if (coll.gameObject.GetInstanceID() == playerHead.GetInstanceID() && !triggered3 && this.name.Equals("Zone3")) {
			triggered3 = true;
			events.GetComponent<LevelEvents3> ().triggerZone3 ();
		}
	}

	public void OnTriggerExit(Collider other) {
		if (!colliding) {
			return;
		}
		colliding = null;
	}
}
