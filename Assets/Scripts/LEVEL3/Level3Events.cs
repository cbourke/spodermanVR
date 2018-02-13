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

	private float moveDist = 35.25f;
	private GameObject cameraRig;
	private bool leverActivated;


	void Awake() {
		cameraRig = GameObject.Find ("[CameraRig]");
	}
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (keyObtained && truckMoving && truck.transform.position.x <= moveDist) {
			truck.transform.Translate (truck.transform.right * Time.deltaTime);
			cameraRig.transform.Translate (truck.transform.right * Time.deltaTime);
		}
		if (!leverActivated && lever.transform.rotation.eulerAngles.x < -40f) {
			moveDist = 48f;
		}
		if (truck.transform.position.x >= 47f) {
			int nextScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene ().buildIndex + 1;
			UnityEngine.SceneManagement.SceneManager.LoadScene (nextScene);
		}
	}

}
