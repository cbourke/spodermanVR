using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerFist : MonoBehaviour
{
	public GameObject Fist;
	public SteamVR_TrackedObject trackedObj;
	private Material valid;

	void Awake ()
	{
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
		valid = (Material)Resources.Load ("Materials/ValidPreviewNode");
	}

	public void createFist ()
	{
		Fist = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		Fist.transform.SetParent (this.transform);
		Fist.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
		Fist.AddComponent (typeof(Rigidbody));
		Fist.GetComponent<Rigidbody> ().useGravity = false;
		Fist.GetComponent<Rigidbody> ().isKinematic = true;
		Fist.transform.localPosition = new Vector3 (0.0f,0.0f,0.1f);
		Fist.AddComponent <IgnoreFistCollisions>();
		Fist.GetComponent<Renderer> ().material = valid;
		//Fist.AddComponent (typeof(SphereCollider));
		//Fist.GetComponent<SphereCollider> ().radius = 2;
		//Fist.tag = "Fistable"; 	//If we want something to be able to be punched in later release. "Fistable" or "Punchable"?
	}

	// Update is called once per frame
	void Update ()
	{
		if (this.GetComponent<FunctionController> ().currentMode.ToString () == "Fist") {
			if (!Fist) {
				createFist ();
			}
		} else {
			DestroyImmediate (Fist, true);
		}
	}
}
