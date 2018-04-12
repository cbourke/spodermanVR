using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnShot : MonoBehaviour {

	public bool shot;

	void Update() {
		if (shot)
			transform.Rotate (Vector3.up , 40 * Time.deltaTime);
	}

	public void OnTriggerEnter(Collider other) {
		shot = true;
		Invoke ("destroyMe",2);
	}

	private IEnumerator rotateDie() {
		while (true) {
			this.transform.Rotate (0f,10f,0f);
			yield return new WaitForSeconds(0.01f);
		}
	}

	private void destroyMe() {
		this.gameObject.SetActive (false);
	}

}
