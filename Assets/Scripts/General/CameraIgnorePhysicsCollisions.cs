using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraIgnorePhysicsCollisions : MonoBehaviour {

	// Use this for initialization
	void Awake() {
		Physics.IgnoreLayerCollision (LayerMask.NameToLayer("CameraZoneCollisions") , 0 , true);
	}

	public static CameraIgnorePhysicsCollisions FindMe() {
		return  GameObject.FindObjectOfType<CameraIgnorePhysicsCollisions>();
	}
}
