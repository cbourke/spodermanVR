using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseballBat : MonoBehaviour {

	public AudioClip smack;
	// Use this for initialization
	void Start () {
		smack = (AudioClip)Resources.Load ("Audio/General/homerun");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnCollisionEnter(Collision other) {
		if (other.gameObject.CompareTag("Projectile")) {
			other.gameObject.GetComponent<bulletBehavior> ().update = () => {

				other.gameObject.transform.position -= other.transform.forward * 10.0f * Time.deltaTime;
			};
			//Vector3 currVelocity = other.collider.attachedRigidbody.velocity;
			//other.collider.attachedRigidbody.velocity = Vector3.zero;
			//other.collider.attachedRigidbody.velocity = -currVelocity;
		}
		if (other.gameObject && other.gameObject.CompareTag("Badguy")) {
			Vector3 direction = -other.contacts [0].normal;
			other.rigidbody.AddForceAtPosition (direction * 5000f , other.contacts[0].point);
			GetComponent<AudioSource> ().Play ();
		}
	}
}
