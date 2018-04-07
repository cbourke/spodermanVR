using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level4ZoneTriggers : MonoBehaviour {

	public GameObject colliding;
	public GameObject headEventCollider;
	public Level4Events level4Events;

	private bool triggered1;
	private bool triggered2;
	private bool triggered3;

	// Use this for initialization
	void Awake() {
		level4Events = Level4Events.FindMe ();
		headEventCollider = CameraIgnorePhysicsCollisions.FindMe ().gameObject;
	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnTriggerEnter (Collider coll) {
		colliding = coll.attachedRigidbody.gameObject;
		if (!triggered1 && coll.gameObject.GetInstanceID() == headEventCollider.GetInstanceID() && this.name.Equals("Zone1")) {
			triggered1 = true;
			level4Events.TriggerZone1 ();
		}

		if (!triggered2 && coll.gameObject.GetInstanceID() == headEventCollider.GetInstanceID()  && this.name.Equals("Zone2")) {
			
		}

		if (!triggered3 && coll.gameObject.GetInstanceID() == headEventCollider.GetInstanceID()  && this.name.Equals("Zone3")) {
			triggered3 = true;
			level4Events.TriggerZone3 ();
		}
	}

	public void OnTriggerExit(Collider other) {
		if (!colliding) {
			return;
		}
		colliding = null;
	}
}
