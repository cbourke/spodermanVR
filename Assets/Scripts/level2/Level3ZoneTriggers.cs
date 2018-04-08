using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3ZoneTriggers : MonoBehaviour {

	public GameObject colliding;
	public GameObject playerHead;
	private LevelEvents3 events;
//	private bool triggered1;
//	private bool triggered2;
//	private bool triggered3;
	// Use this for initialization
	void Awake () {
		playerHead = CameraIgnorePhysicsCollisions.FindMe ().gameObject;
		events = LevelEvents3.FindMe ();
//		triggered1 = false;
//		triggered2 = false;
//		triggered3 = false;
	}

	public void OnTriggerEnter (Collider coll) {
		colliding = coll.attachedRigidbody.gameObject;
		if (coll.gameObject.GetInstanceID() == playerHead.GetInstanceID()) {
			switch (this.name) {
			case "Zone1":
				events.triggerZone1 ();
				break;
			case "Zone2":
				events.triggerZone2 ();
				break;
//			case "Zone3":
//				events.triggerZone3 ();
//				break;
			default:
				return;
			}
			Destroy (this.gameObject);
		}
	}

	public void OnTriggerExit(Collider other) {
		if (!colliding) {
			return;
		}
		colliding = null;
	}
}
