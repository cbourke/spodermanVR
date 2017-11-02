using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollisions : MonoBehaviour {

	void Awake() {
		Physics.IgnoreLayerCollision (8,0,true);

	}
}
