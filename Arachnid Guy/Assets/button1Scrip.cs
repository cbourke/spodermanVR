using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button1Scrip : MonoBehaviour {
	public GameObject target;
	private bool activated;

	// Use this for initialization
	void Start () {
		activated = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!target) {
			GetComponent<Renderer> ().material.mainTexture = (Texture)Resources.Load ("Textures/buttonGreen");

		}
	}
}
