using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletBehavior : MonoBehaviour {

	private GameObject playerHead;
	private float speed = 10f;
	Vector3 targetLoc;
	// Use this for initialization
	void Awake() {
		GetComponent<Rigidbody> ().freezeRotation = true;
	}
	void Start () {
		playerHead = HeadColliderHandler.FindMe ().gameObject;
		targetLoc = playerHead.transform.position;
		transform.LookAt (targetLoc);
		StartCoroutine (kill());
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.position += transform.forward * speed * Time.deltaTime;
	}

	public void OnTriggerEnter (Collider coll) {
		Debug.Log (coll.gameObject.name);
		if (coll.gameObject.GetInstanceID() == playerHead.GetInstanceID()) {
			HeadColliderHandler.FindMe ().Damage (10f);
			Debug.Log ("BULLET HIT PLAYER");
		}
		foreach(Transform child in gameObject.transform) {
			Destroy (child.gameObject);
		}
		Destroy (gameObject);
	}

	private IEnumerator kill() {
		yield return new WaitForSeconds (4.0f);
		foreach(Transform child in gameObject.transform) {
			Destroy (child.gameObject);
		}
		Destroy (gameObject);
	}
}
