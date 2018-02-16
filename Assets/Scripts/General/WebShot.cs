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
//		shotForce = 2000.0f;

	}

	void Update () {

	}
	



	public void Shoot () {
		shot = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		shot.name = "WebShot";
		Vector3 forOffset = this.transform.position + this.transform.forward * 0.15f;
		//shot.transform.position = this.transform.position + new Vector3(0.5f,0,0);
		shot.transform.position = forOffset;
		shot.AddComponent<Rigidbody> ();
		shot.transform.localScale = new Vector3 (0.07f,0.07f,0.07f);
		shot.AddComponent<WebShotCollider> ();
		shot.GetComponent<Rigidbody> ().AddForce (this.transform.forward * shotForce);
	}

}
