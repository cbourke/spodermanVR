using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Events : MonoBehaviour {

	public bool keyObtained;
	public GameObject window1;
	public GameObject window1text;
	public bool truckMoving;
	public GameObject truck;
	public GameObject lever;
	public GameObject gate;

	private float moveDist = 35.25f;
	public GameObject cameraRig;
	private bool leverActivated;
	private float gateMoveDist = -1f;
	private bool stopGate;


	void Awake() {
		cameraRig = GameObject.Find ("[CameraRig]");
		leverActivated = false;
		stopGate = false;
	}
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
		if (!leverActivated && lever.transform.rotation.eulerAngles.x < -40f) {
//			moveDist = 48f;
			leverActivated = true;
		}
		if (truck.transform.position.x >= 47f) {
			int nextScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene ().buildIndex + 1;
			UnityEngine.SceneManagement.SceneManager.LoadScene (nextScene);
		}
		if (!stopGate && leverActivated) {
			gate.transform.Translate (-gate.transform.forward * Time.deltaTime);
			if (gate.transform.position.x <= gateMoveDist) {
				stopGate = true;
				moveDist = 48;
			}
		}
	}

	void LateUpdate () {
		if (keyObtained && truckMoving && truck.transform.position.x <= moveDist) {
			truck.transform.Translate (truck.transform.right * Time.deltaTime);
			cameraRig.transform.Translate (truck.transform.right * Time.deltaTime);
		}
	}



}
