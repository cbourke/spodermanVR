using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuLayerCollisions : MonoBehaviour {


	// Use this for initialization
	void Awake () {
		Physics.IgnoreLayerCollision (9 , 0 , true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
