﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebShotCollider : MonoBehaviour {

	void Awake() {
		this.GetComponent<Collider> ().isTrigger = true;
		this.GetComponent<Rigidbody> ().collisionDetectionMode = CollisionDetectionMode.Continuous;
	}
	void Start () {
		StartCoroutine (DestroyShot());
	}

	public void OnTriggerEnter(Collider other) {
		//add sticky element here. Implement as needed
		if (other.gameObject.CompareTag("Badguy")) {
			other.gameObject.GetComponent<BaseEnemy> ().SlowSpeed(0.5f);
		}
		Destroy (this.gameObject);
	}

	//DEPRECATED since switch to trigger collider, rather than physics based collision
//	public void OnCollisionEnter(Collision other) {
////		if (other.gameObject.GetComponent<Rigidbody>() && !other.gameObject.GetComponent<Rigidbody>().isKinematic) {
////			//this block of code is redundant, collisions between two rigidbodies already apply a physics force.
//////			Vector3 direction = this.GetComponent<Rigidbody> ().velocity / 10;
//////			Debug.Log (direction);
//////			other.rigidbody.AddForceAtPosition ( direction , other.contacts[0].point);
////
////		}

		
	private IEnumerator DestroyShot() {
		yield return new WaitForSeconds(5.0f);
		Destroy (this.gameObject);
	}
		

}
