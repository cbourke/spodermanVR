using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadColliderHandler : MonoBehaviour {

	private int headLayer;

	private int cameraZoneLayer;
	private GameObject collObj;
	private GameObject world;
	// Use this for initialization
	void Awake() {
		headLayer = LayerMask.NameToLayer ("Default");
		cameraZoneLayer = LayerMask.NameToLayer ("CameraZoneCollisions");
		Physics.IgnoreLayerCollision (headLayer , cameraZoneLayer , true);
		world = GameObject.Find ("WorldNodeTracker");
	}

	void Start () {

	}

	public void OnTriggerEnter (Collider coll) {
		SetCollidingObject (coll.attachedRigidbody.gameObject);
	}

	public void OnTriggerStay(Collider coll) {

	}

	public void OnTriggerExit (Collider coll) {
		if (!collObj) {
			return;
		}
		collObj = null;
	}

	private void SetCollidingObject (GameObject obj) {
		if (collObj || !obj.GetComponent<Rigidbody>()) {
			return;
		}
		collObj = obj;
	}



	// Update is called once per frame
	void Update () {
		if (collObj != null) {
			//blur out camera here
		}
	}
}
