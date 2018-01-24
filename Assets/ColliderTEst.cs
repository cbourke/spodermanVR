using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTEst : MonoBehaviour {

	public bool create;

	private GameObject shot;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (create) {
			create = false; 
			shot = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			shot.name = "WebShot";
			shot.transform.position = this.transform.position;
			shot.AddComponent<Rigidbody> ();
			shot.transform.localScale = new Vector3 (0.07f,0.07f,0.07f);
			shot.GetComponent<Rigidbody> ().collisionDetectionMode = CollisionDetectionMode.Continuous;
			shot.GetComponent<SphereCollider> ().isTrigger = true;
			shot.GetComponent<Rigidbody> ().useGravity = true;
			shot.AddComponent<WebShotCollider> ();
		}
	}
}
