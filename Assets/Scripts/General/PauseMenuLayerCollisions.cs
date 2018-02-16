using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuLayerCollisions : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		//Pause menu ignores all collisions with default layer
		Physics.IgnoreLayerCollision (LayerMask.NameToLayer("PauseMenu") , 0 , true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
