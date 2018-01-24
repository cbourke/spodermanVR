using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEvents1_1 : MonoBehaviour {

	public GameObject endMessageHandle;
	public GameObject message;
	public GameObject leftController;
	public GameObject rightController;

	private bool messagePlayed;

	void Awake() {
		messagePlayed = false;
		message.SetActive (false);
	}

	void Start() {
		FunctionController leftFunc = leftController.GetComponent<FunctionController> ();
		FunctionController rightFunc = rightController.GetComponent<FunctionController> ();
		leftFunc.fistEnabled = false;
		leftFunc.shotEnabled = false;
		leftFunc.retractEnabled = false;
		leftFunc.ropeEnabled = false;
		rightFunc.fistEnabled = false;
		rightFunc.shotEnabled = false;
		rightFunc.retractEnabled = false;
		rightFunc.ropeEnabled = false;
	}

	public void showMessage() {
		if (!messagePlayed) {
			messagePlayed = true;
			message.SetActive (true);
			//message.GetComponent<Animator>().Play ("SignBlinkIn");
			message.GetComponent<AudioSource> ().Play ();
		}
	}



}
