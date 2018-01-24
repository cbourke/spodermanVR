using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtChecker : MonoBehaviour {

	public GameObject headset;
	public GameObject visibleObj;

	private int layerMask;

	void Awake() {
		headset = GameObject.Find ("Camera (eye)");
		layerMask = 1 << 8;
		layerMask = ~layerMask;
	}

	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;	
		if (Physics.Raycast (headset.transform.position, headset.transform.forward, out hit, 100, layerMask)) {
			visibleObj = hit.collider.gameObject;
		} else {
			visibleObj = null;
		}
	}

	public GameObject getObject() {
		return visibleObj;
	}
		
}
