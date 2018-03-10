using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreFistCollisions : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		Collider coll = this.transform.parent.GetComponent<Collider> ();
		Physics.IgnoreCollision (coll , this.GetComponent<Collider>());
		this.GetComponent<Rigidbody> ().useGravity = false;
		this.GetComponent<Rigidbody> ().isKinematic = true;
	}

	public void OnCollisionEnter(Collision other) {
		if (other.gameObject.GetComponent<Rigidbody>() && !other.gameObject.GetComponent<Rigidbody>().isKinematic) {
			Vector3 direction = other.transform.position - this.transform.position;
			direction = direction * 2000;
			if (other.gameObject.CompareTag ("Badguy"))
				other.gameObject.GetComponent<BaseEnemy> ().Damage ();
			other.rigidbody.AddForceAtPosition (direction , other.contacts[0].point);
		}
//		if (other.gameObject.tag == "BadGuy") {
//			StartCoroutine (Blink(other.gameObject));
//		}
//		if (world.GetComponent<ControllerCommWorld>().checkRetractingObj(this,)) {
//
//		} TODO: figure out a way to get the controller this collider is attached to.
	
	}

//	private IEnumerator Blink(GameObject obj) {
//		int intervalCount = 10;
//		float blinkInterval = 0.05f;
//		while (intervalCount >= 0) {
//			obj.GetComponent<Renderer> ().enabled = false;
//			intervalCount--;
//			yield return new WaitForSeconds (blinkInterval);
//			obj.GetComponent<Renderer> ().enabled = true;
//			yield return new WaitForSeconds (blinkInterval);
//		}
//		obj.SetActive (false);
//
//	}

}
