using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuWorld : MonoBehaviour {

	public GameObject PMenu;
	public GameObject head;
	public float headOffset;
	public bool paused;
	public List<GameObject> list = new List<GameObject> ();
	private AudioClip openNoise;
	private AudioClip closeNoise;
//	public GameObject leftController;
//	public GameObject rightController;

	void Awake() {
		PMenu = (GameObject)Resources.Load ("Prefabs/PauseMenu");
		openNoise = (AudioClip)Resources.Load ("Audio/General/pauseMenuOpen");
		closeNoise = (AudioClip)Resources.Load ("Audio/General/pauseMenuClose");
	}

	void Start() {
		head = GameObject.Find ("Camera (eye)");
		paused = false;
//		leftController = GameObject.Find ("Controller(left)");
//		rightController = GameObject.Find ("Controller(right)");
	}

	public void ShowMenu(GameObject sendingController, GameObject otherController)
	{
		//cancelRetract ();
//		sendingController.GetComponent<FunctionController>().climbEnabled = false;
//		sendingController.GetComponent<FunctionController>().ropeEnabled = false;
//		sendingController.GetComponent<FunctionController>().retractEnabled = false;
//		sendingController.GetComponent<FunctionController>().fistEnabled = false;
//		sendingController.GetComponent<FunctionController>().shotEnabled = false;
		if (list.Count != 0) {
			return;
		}

		sendingController.GetComponent<FunctionController>().climbEnabled = false;
		sendingController.GetComponent<FunctionController>().ropeEnabled = false;
		sendingController.GetComponent<FunctionController>().retractEnabled = false;
		sendingController.GetComponent<FunctionController>().fistEnabled = false;
		sendingController.GetComponent<FunctionController>().shotEnabled = false;

		otherController.GetComponent<FunctionController>().climbEnabled = false;
		otherController.GetComponent<FunctionController>().ropeEnabled = false;
		otherController.GetComponent<FunctionController>().retractEnabled = false;
		otherController.GetComponent<FunctionController>().fistEnabled = false;
		otherController.GetComponent<FunctionController>().shotEnabled = false;
		paused = true;

		list.Add (Instantiate(PMenu));
		list[0].transform.position = head.transform.position;
		list[0].transform.position += head.transform.forward * headOffset;
		list[0].transform.rotation = Quaternion.identity;
		list[0].transform.Rotate (90f,head.transform.rotation.eulerAngles.y - 180f,0f);

		list [0].GetComponent<AudioSource> ().clip = openNoise;
		list [0].GetComponent<AudioSource> ().Play ();
		//PMenu.transform.rotation = Quaternion.FromToRotation (PMenu.transform.forward,head.transform.forward);
		//PMenu.transform.LookAt(head.transform);

		//GameObject.Find ("Panel").SetActive (true);
		//PMenuTransform.position = Vector3.Lerp(trackedObj.transform.position, hitPoint, .5f);

	}

	public void revertButtons (Texture[] textures) {
		//List<GameObject> children;
		if (list.Count > 0) {
			foreach (Transform child in list[0].transform) {
				if (child.name.Equals("Continue"))  
					child.GetComponent<Renderer> ().material.mainTexture = textures [0];
				if (child.name.Equals("Restart")) 
					child.GetComponent<Renderer> ().material.mainTexture = textures [2];
				if (child.name.Equals("Quit"))
					child.GetComponent<Renderer> ().material.mainTexture = textures [4];
			}
		}
	}

//	public void cancelRetract() {
//		leftController.GetComponent<ControllerRetract> ().retracting = false;
//		rightController.GetComponent<ControllerRetract> ().retracting = false;
//		if (leftController.GetComponent<ControllerRetract> ().retractobj) {
//			leftController.GetComponent<ControllerRetract> ().retractobj.GetComponent<Rigidbody> ().isKinematic = false;
//			leftController.GetComponent<ControllerRetract> ().retractobj.GetComponent<Rigidbody> ().useGravity = true;
//			leftController.GetComponent<ControllerRetract> ().retractobj = null;
//		}
//		if (leftController.GetComponent<ControllerRetract> ().retractobj) {
//			leftController.GetComponent<ControllerRetract> ().retractobj.GetComponent<Rigidbody> ().isKinematic = false;
//			leftController.GetComponent<ControllerRetract> ().retractobj.GetComponent<Rigidbody> ().useGravity = true;
//			leftController.GetComponent<ControllerRetract> ().retractobj = null;
//		}
//	}

	public void HideMenu()
	{
		

		//GameObject.Find ("Panel").SetActive (false);
		//PMenu = GameObject.Find ("PauseMenu");
		//PMenu
		if (list.Count != 1) {
			return;
		}
		list [0].GetComponent<AudioSource> ().clip = closeNoise;
		list [0].GetComponent<AudioSource> ().Play ();
		list[0].GetComponent<MeshRenderer> ().enabled = false;
		foreach (Transform child in list[0].transform) {
			child.GetComponent<MeshRenderer> ().enabled = false;
		}
		StartCoroutine (Destroy());


	}

	private IEnumerator Destroy() {
		yield return new WaitForSecondsRealtime(.201f);
		paused = false;
		DestroyImmediate(list[0],false);
		list.Clear ();
	}

}
