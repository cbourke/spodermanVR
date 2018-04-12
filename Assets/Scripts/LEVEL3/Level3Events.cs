using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Events : MonoBehaviour {

	public bool keyObtained;
	public GameObject window1;
	public GameObject window2;
	public GameObject window3;
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
	public Texture[] window1Feed;
	public Texture[] window2Feed;
	public Texture[] window3Feed;
	public int nextLevel;


	void Awake() {
		cameraRig = GameObject.Find ("[CameraRig]");
		leverActivated = false;
		stopGate = false;
		util = EventUtil.FindMe ();


	}
	// Use this for initialization
	void Start () {
		util.GetWindowControllerFromWindow(window1).updateArray (window1Feed);
		util.GetWindowControllerFromWindow(window2).updateArray (window2Feed);
		util.GetWindowControllerFromWindow(window3).updateArray (window3Feed);
		util.GetWindowControllerFromWindow (window3).ChangeLock (2);
		window2.SetActive (false);
		window3.SetActive (false);
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
			UnityEngine.SceneManagement.SceneManager.LoadScene (nextLevel);
		}
		if (!stopGate && leverActivated) {
			gate.transform.Translate (gate.transform.forward * Time.deltaTime * 2f);
			if (gate.transform.position.z >= gateMoveDist) {
				stopGate = true;
				moveDist = 48;
			}
		}
		if (leverActivated)
			util.GetWindowControllerFromWindow (window3).ChangeLock (10);	//FIXME stuff
	}

	void LateUpdate () {
		if (truckMovingL || truckMovingR) {

			if (keyObtained && truckMovingL && truck.transform.position.x <= moveDist) {
				truck.transform.Translate (truck.transform.right * Time.deltaTime * 2f);
				util.getLeftController().GetComponent<ControllerGrab> ().startingControllerPosition += truck.transform.right * Time.deltaTime * 2f;
			} else {
				if (keyObtained && truckMovingR && truck.transform.position.x <= moveDist) {
					truck.transform.Translate (truck.transform.right * Time.deltaTime * 2f);
					util.getRightController().GetComponent<ControllerGrab> ().startingControllerPosition += truck.transform.right * Time.deltaTime * 2f;
				}
			}
		}
		if (!window3.activeSelf && truck.transform.position.x >= moveDist) {
			window3.SetActive (true);
			util.GetWindowControllerFromWindow (window3).ChangeLock (1);
		}

	}



}
