using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelComplete0_3 : MonoBehaviour {
	public GameObject brother;
	public GameObject face;
	public GameObject speechBubble;
	public GameObject goal;

	// Use this for initialization
	void Start () {
		goal.GetComponent<LevelBridge> ().open = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnCollisionEnter(Collision other) {
		if (other.gameObject.Equals(brother)) {
			face.GetComponent<Renderer>().material = (Material)Resources.Load ("Materials/savedFace");
			speechBubble.GetComponent<Renderer>().material = (Material)Resources.Load ("Materials/thankYou");
			goal.GetComponent<LevelBridge> ().open = true;
		}
	}
}
