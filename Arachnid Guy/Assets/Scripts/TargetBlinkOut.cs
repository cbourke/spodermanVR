using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBlinkOut : MonoBehaviour {
	public float intervalCount = 10f;
	public float blinkInterval = 0.05f;

	void Start() {
	}

	public void OnCollisionEnter (Collision other) {
		if (other.gameObject.name == "WebShot") {
			StartCoroutine (Blink(this.gameObject));
		}
	}

	private IEnumerator Blink(GameObject obj) {
		while (intervalCount >= 0) {
			obj.GetComponent<Renderer> ().enabled = false;
			intervalCount--;
			yield return new WaitForSeconds (blinkInterval);
			obj.GetComponent<Renderer> ().enabled = true;
			yield return new WaitForSeconds (blinkInterval);
		}
		obj.SetActive (false);

	}
}
