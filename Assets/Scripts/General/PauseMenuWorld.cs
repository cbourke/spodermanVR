using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuWorld : MonoBehaviour {

	public GameObject PMenu;
	public GameObject head;
	public float headOffset;
	private List<GameObject> list = new List<GameObject> ();
//	public GameObject leftController;
//	public GameObject rightController;

	void Awake() {
		PMenu = (GameObject)Resources.Load ("Prefabs/PauseMenu");

	}

	void Start() {
		head = GameObject.Find ("Camera (eye)");
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

		list.Add (Instantiate(PMenu));
		list[0].transform.position = head.transform.position;
		list[0].transform.position += head.transform.forward * headOffset;
		list[0].transform.rotation = Quaternion.identity;
		list[0].transform.Rotate (90f,head.transform.rotation.eulerAngles.y - 180f,0f);
		//PMenu.transform.rotation = Quaternion.FromToRotation (PMenu.transform.forward,head.transform.forward);
		//PMenu.transform.LookAt(head.transform);

		Debug.Log (head.transform.rotation.eulerAngles.y);
		//GameObject.Find ("Panel").SetActive (true);
		//PMenuTransform.position = Vector3.Lerp(trackedObj.transform.position, hitPoint, .5f);

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
		DestroyImmediate(list[0],false);
		list.Clear ();


	}

}
