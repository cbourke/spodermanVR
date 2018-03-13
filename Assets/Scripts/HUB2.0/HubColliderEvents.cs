using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubColliderEvents : MonoBehaviour {

	private HubEvents hub;
	public GameObject playerHead;
	public GameObject collObj;
	public bool started;
	// Use this for initialization
	void Awake () {
		hub = HubEvents.FindMe ();
		playerHead = CameraIgnorePhysicsCollisions.FindMe ().gameObject;
	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnTriggerEnter(Collider coll) {
		collObj = coll.gameObject;
		if (!started && collObj.gameObject.GetInstanceID() == playerHead.GetInstanceID()) {
			started = true;
			hub.StartHub ();
		}
	}

	public void OnTriggerExit(Collider other) {
		if (!collObj) {
			return;
		}
		collObj = null;
	}
}
