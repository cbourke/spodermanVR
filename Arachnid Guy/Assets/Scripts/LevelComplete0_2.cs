using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelComplete0_2 : MonoBehaviour {
	public GameObject badGuy;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!badGuy.activeInHierarchy) { 
			this.GetComponent<LevelBridge>().open = true;
		}
		else {
			this.GetComponent<LevelBridge>().open = false;
		}
	}
}
