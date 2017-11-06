using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebShot : MonoBehaviour {
	
//	public SteamVR_TrackedObject trackedObj;
	public float shotForce;
	private GameObject shot;


//	private SteamVR_Controller.Device Controller {
//		get { return SteamVR_Controller.Input ((int)trackedObj.index); }
//	}

	// Use this for initialization
	void Start () {
		shotForce = 2.0f;

	}

	void Update () {

	}
	



	public void Shoot () {
		shot = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		shot.name = "WebShot";
		shot.transform.position = this.transform.position;
		shot.AddComponent<Rigidbody> ();
		shot.transform.localScale = new Vector3 (0.1f,0.1f,0.1f);
		shot.GetComponent<Rigidbody> ().collisionDetectionMode = CollisionDetectionMode.Continuous;
		//shot.GetComponent<Collider> ().isTrigger = true;
		shot.AddComponent<WebShotCollider> ();
		shot.GetComponent<Rigidbody> ().AddForce (this.transform.forward * shotForce);
	}

}
