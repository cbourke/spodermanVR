using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class bulletBehavior : MonoBehaviour {

	private GameObject playerHead;
	private float speed = 10f;
	Vector3 targetLoc;
	public Action update;
	// Use this for initialization
	void Awake() {
		GetComponent<Rigidbody> ().freezeRotation = true;
		update = () => {
			this.gameObject.transform.position += this.transform.forward * speed * Time.deltaTime;
		};
	}
	void Start () {
		playerHead = HeadColliderHandler.FindMe ().gameObject;
		targetLoc = playerHead.transform.position;
		transform.LookAt (targetLoc);
		StartCoroutine (kill());
	}
	
	// Update is called once per frame
	void Update () {
		this.update.Invoke ();
		//gameObject.transform.position += transform.forward * speed * Time.deltaTime;
	}

	public void OnTriggerEnter (Collider coll) {
		if (coll.gameObject.GetInstanceID() == playerHead.GetInstanceID()) {
			HeadColliderHandler.FindMe ().Damage (10f);
		}
		foreach(Transform child in gameObject.transform) {
			Destroy (child.gameObject);
		}
		Destroy (gameObject);
	}

	public void OnCollisionEnter (Collision other) {
		if (other.gameObject.CompareTag("Badguy")) {
			Vector3 direction = -other.contacts [0].normal;
			direction = direction * 2000;
			other.rigidbody.AddForceAtPosition (direction , other.contacts[0].point);
		}
	}

	private IEnumerator kill() {
		yield return new WaitForSeconds (4.0f);
		foreach(Transform child in gameObject.transform) {
			Destroy (child.gameObject);
		}
		Destroy (gameObject);
	}
}
