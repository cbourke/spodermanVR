using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraIgnorePhysicsCollisions : MonoBehaviour {

	// Use this for initialization
	void Awake() {
		Physics.IgnoreLayerCollision (10 , 0 , true);
	}
}
