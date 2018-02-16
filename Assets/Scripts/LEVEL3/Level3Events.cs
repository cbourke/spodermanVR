using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Events : MonoBehaviour {

	public bool keyObtained;
	public GameObject window1;
	public GameObject window1text;
	public bool truckMovingL;
	public bool truckMovingR;
	public GameObject truck;
	public GameObject lever;
	public GameObject gate;
	private EventUtil util;
	private float moveDist = 30f;
	public GameObject cameraRig;
	private bool leverActivated;
	private float gateMoveDist = -1f;
	private bool stopGate;


	void Awake() {
		cameraRig = GameObject.Find ("[CameraRig]");
		leverActivated = false;
		stopGate = false;
		util = this.GetComponent<EventUtil> ();

	}
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
		if (!leverActivated && lever.transform.rotation.eulerAngles.x >= 315f && lever.transform.rotation.eulerAngles.x <= 320f) {
//			moveDist = 48f;
			leverActivated = true;
			util.cancelRetract ();

		}
		if (truck.transform.position.x >= 47f) {
//			int nextScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene ().buildIndex + 1;
			UnityEngine.SceneManagement.SceneManager.LoadScene (0);
		}
		if (!stopGate && leverActivated) {
			gate.transform.Translate (gate.transform.forward * Time.deltaTime);
			if (gate.transform.position.z >= gateMoveDist) {
				stopGate = true;
				moveDist = 48;
			}
		}
	}

	void LateUpdate () {
		if (truckMovingL || truckMovingR) {
			if (keyObtained && truckMovingL && truck.transform.position.x <= moveDist) {
				truck.transform.Translate (truck.transform.right * Time.deltaTime);
				util.leftController.GetComponent<ControllerGrab> ().startingControllerPosition += truck.transform.right * Time.deltaTime;
			} else {
				if (keyObtained && truckMovingR && truck.transform.position.x <= moveDist) {
					truck.transform.Translate (truck.transform.right * Time.deltaTime);
					util.rightController.GetComponent<ControllerGrab> ().startingControllerPosition += truck.transform.right * Time.deltaTime;
				}
			}
		}

	}



}
