using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebShotCollider : MonoBehaviour {


	void Start () {
		StartCoroutine (DestroyShot());
	}

	public void OnTriggerEnter(Collider other) {
		//add sticky element here. Implement as needed
		Debug.Log("SPLAT");
		Destroy (this.gameObject);
	}

	public void OnCollisionEnter(Collision other) {
//		if (other.gameObject.GetComponent<Rigidbody>() && !other.gameObject.GetComponent<Rigidbody>().isKinematic) {
//			//this block of code is redundant, collisions between two rigidbodies already apply a physics force.
////			Vector3 direction = this.GetComponent<Rigidbody> ().velocity / 10;
////			Debug.Log (direction);
////			other.rigidbody.AddForceAtPosition ( direction , other.contacts[0].point);
//
//		}

		//TODO: Decal system?
//		RaycastHit hit;
//		if (Physics.Raycast(this.transform.position,this.GetComponent<Rigidbody>().velocity,out hit)) {
//			var hitRotation = Quaternion.FromToRotation (Vector3.up, hit.normal);
//
//		}
		Destroy (this.gameObject);
	}
		
	private IEnumerator DestroyShot() {
		yield return new WaitForSeconds(5.0f);
		Destroy (this.gameObject);
	}
		

}
