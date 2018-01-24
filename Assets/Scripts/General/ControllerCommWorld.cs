using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerCommWorld : MonoBehaviour {

	public GameObject leftController;
	public GameObject rightController;

	private GameObject leftRetractObj;
	private GameObject rightRetractObj;

	void Awake() {
		leftController = GameObject.Find ("Controller (left)");
		rightController = GameObject.Find ("Controller (right)");
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setRetractObj(GameObject sendingController , GameObject pullObj) {
		if (sendingController.GetInstanceID() == leftController.GetInstanceID()) {
			leftRetractObj = pullObj;
			return;
		}
		if (sendingController.GetInstanceID() == rightController.GetInstanceID()) {
			rightRetractObj = pullObj;
			return;
		}
		else
			return;
	}

	public bool checkRetractingObj(GameObject pullObj) {
		if (pullObj.GetInstanceID() == rightRetractObj.GetInstanceID() || pullObj.GetInstanceID() == leftRetractObj.GetInstanceID()) {
			return true;
		} else
			return false;
	}
}
