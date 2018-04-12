using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lvl1ZoneHandler : MonoBehaviour {

	public GameObject colliding;
	public GameObject headEventCollider;
	public LevelEvents1_1 events;

	private bool triggered1 = false;
	// Use this for initialization
	void Start () {
		headEventCollider = CameraIgnorePhysicsCollisions.FindMe ().gameObject;
		events = LevelEvents1_1.FindMe ();
	}

	public void OnTriggerEnter (Collider coll) {
		if (!triggered1 && coll.gameObject.GetInstanceID() == headEventCollider.GetInstanceID() && this.name.Equals("Zone1")) {
			triggered1 = true;
			events.showMessage ();
			this.gameObject.SetActive (false);
		}
	}
}
