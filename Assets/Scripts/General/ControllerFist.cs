using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerFist : MonoBehaviour
{
	public GameObject Fist;
	public SteamVR_TrackedObject trackedObj;
	private Material fistMat;

	void Awake ()
	{
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
		fistMat = (Material)Resources.Load ("Materials/General/Fist");
	}

	public void createFist ()
	{
		Fist = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		Fist.name = "Fist";
		Fist.transform.SetParent (this.transform);
		Fist.AddComponent (typeof(Rigidbody));
		Fist.transform.localScale = new Vector3 (0.2f, 0.2f, 0.2f);
		Fist.transform.localPosition = new Vector3 (0.0f,0.0f,0.05f);
		Fist.AddComponent <IgnoreFistCollisions>();
		Fist.GetComponent<Renderer> ().material = fistMat;
	}

	// Update is called once per frame
	void Update ()
	{
		if (this.GetComponent<FunctionController> ().currentMode.ToString () == "Fist") {
			if (!Fist) {
				createFist ();
			}
		} else {
			DestroyImmediate (Fist, false);
		}
	}
}
