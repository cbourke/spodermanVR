using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnShot : MonoBehaviour {

	public bool shot;
	public float rotateSpeed;

	void Update() {
		if (shot) {
			transform.Rotate (Vector3.forward, rotateSpeed * Time.deltaTime);
		}
	}

	public void OnTriggerEnter(Collider other) {
		this.GetComponent<AudioSource> ().Play ();
		rotateSpeed += 180f;
		shot = true;
		Invoke ("destroyMe",2);
	}

	private void destroyMe() {
		this.gameObject.SetActive (false);
	}

}
