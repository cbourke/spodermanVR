using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebShotCollider : MonoBehaviour {


	void Start () {
		StartCoroutine (DestroyShot());
	}

	public void OnCollisionEnter(Collision other) {
		if (other.gameObject.GetComponent<Rigidbody>() && !other.gameObject.GetComponent<Rigidbody>().isKinematic) {
			other.rigidbody.AddForceAtPosition (this.GetComponent<Rigidbody>().velocity , other.contacts[0].point);
		}
		//TODO: Decal system?
//		RaycastHit hit;
//		if (Physics.Raycast(this.transform.position,this.GetComponent<Rigidbody>().velocity,out hit)) {
//			var hitRotation = Quaternion.FromToRotation (Vector3.up, hit.normal);
//
//		}
		Destroy (this.gameObject);
	}
		
	private IEnumerator DestroyShot() {
		yield return new WaitForSecondsRealtime(5.0f);
		Destroy (this.gameObject);
	}

}
