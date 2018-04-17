using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreFistCollisions : MonoBehaviour {
	private EventUtil util;


	void Awake () {
		Collider coll = this.transform.parent.GetComponent<Collider> ();
		Physics.IgnoreCollision (coll , this.GetComponent<Collider>());
		this.GetComponent<Rigidbody> ().useGravity = false;
		this.GetComponent<Rigidbody> ().isKinematic = true;
		util = EventUtil.FindMe ();
	}

	public void OnCollisionEnter(Collision other) {
		if (other.gameObject.CompareTag("Boss")) {
			other.rigidbody.useGravity = true;
			other.rigidbody.isKinematic = false;
			other.gameObject.GetComponent<Level4Boss> ().Death ();
		}
		
		if (other.gameObject.CompareTag("Projectile")) {
			other.gameObject.GetComponent<bulletBehavior> ().update = () => {
				
				other.gameObject.transform.position -= other.transform.forward * 10.0f * Time.deltaTime;
			};
			util.playClip (this.transform.parent.gameObject , (AudioClip)Resources.Load("Audio/General/shot"));
		}
		else if (other.gameObject.GetComponent<Rigidbody>() && !other.gameObject.GetComponent<Rigidbody>().isKinematic) {
			Vector3 direction = -other.contacts [0].normal;
			direction = direction * 2000;
			other.rigidbody.AddForceAtPosition (direction , other.contacts[0].point);
		}
			
	
	}

	//On punch, could blink object out of existence, but this blink effect is disorienting and generally unpleasant to
	//	see in VR. No longer used.
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
