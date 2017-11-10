using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHighlighter : MonoBehaviour {

	private GameObject collidingObject;

	private void SetCollidingObject(Collider col)
	{
		if (collidingObject || !col.GetComponent<Rigidbody>())
		{
			return;
		}
		collidingObject = col.gameObject;


	}

	public void OnTriggerEnter(Collider other) {
		SetCollidingObject (other);
	}

	public void OnTriggerStay(Collider other) {
		SetCollidingObject (other);
	}

	public void OnTriggerExit(Collider other) {
		if (!collidingObject) {
			return;
		}
		collidingObject = null;
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (collidingObject && collidingObject.GetComponent<SteamVR_TrackedObject>()) {
			if (GetComponent<Renderer> ().material.mainTexture != (Texture)Resources.Load ("Textures/spideyButton2")) {
				GetComponent<Renderer> ().material.mainTexture = (Texture)Resources.Load ("Textures/spideyButton2");
			}
		}
		else if(!collidingObject) {
			if (GetComponent<Renderer> ().material.mainTexture != (Texture)Resources.Load ("Textures/spideyButton1")) {
				GetComponent<Renderer> ().material.mainTexture = (Texture)Resources.Load ("Textures/spideyButton1");
			}
		}

	}
}
