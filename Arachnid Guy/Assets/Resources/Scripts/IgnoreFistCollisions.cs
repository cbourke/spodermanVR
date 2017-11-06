using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreFistCollisions : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		Collider coll = this.transform.parent.GetComponent<Collider> ();
		Physics.IgnoreCollision (coll , this.GetComponent<Collider>());
	}

	public void OnCollisionEnter(Collision other) {
		if (other.gameObject.GetComponent<Rigidbody>() && !other.gameObject.GetComponent<Rigidbody>().isKinematic) {
			Vector3 direction = other.transform.position - this.transform.position;
			direction = direction * 2000;
			other.rigidbody.AddForceAtPosition (direction , other.contacts[0].point);
			Debug.Log (direction);
		}
	}

}
