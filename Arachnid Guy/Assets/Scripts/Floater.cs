using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour {
	private Vector3 startingPos;
	private Vector3 bottom;
	private Vector3 dec;
	private bool goingDown;
	private float offset;

	// Use this for initialization
	void Start () {
		startingPos = transform.position;
		bottom = transform.position - new Vector3 (0f,0.05f,0f);
		dec = new Vector3 (0f, 0.0003f, 0f);
		goingDown = true;
		offset = Vector3.Distance(startingPos , bottom);
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position == startingPos) {
			if (goingDown) {
				transform.position -= dec;
			} else {
				goingDown = true;
				transform.position -= dec;
			}
		} else if (Vector3.Distance(startingPos , transform.position) > offset) {
			if (goingDown) {
				goingDown = false;
				transform.position += dec;
			} else {
				transform.position += dec;
			}
		} else {
			if (goingDown) {
				transform.position -= dec;
			} else {
				transform.position += dec;
			}
		}



	}
}
