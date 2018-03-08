using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubColliderEvents : MonoBehaviour {

	private HubEvents hub;

	// Use this for initialization
	void Awake () {
		hub = HubEvents.FindMe ();
	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnTriggerEnter(Collider coll) {
		GameObject collObj = coll.gameObject;
		if (collObj.name == "StartZone") {
			hub.StartHub ();
		}
	}
}
