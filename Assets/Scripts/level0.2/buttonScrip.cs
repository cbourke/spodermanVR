using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonScrip : MonoBehaviour {
	public GameObject target;
	public bool activated;

	// Use this for initialization
	void Start () {
		activated = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!activated && !target.activeInHierarchy) {
			GetComponent<Renderer> ().material.mainTexture = (Texture)Resources.Load ("Textures/level0.2/buttonGreen");
			activated = true;
		}
	}
}
