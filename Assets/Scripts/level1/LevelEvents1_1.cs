using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEvents1_1 : MonoBehaviour {

//	public GameObject endMessageHandle;
	public GameObject message;
	public GameObject leftController;
	public GameObject rightController;
	private bool messagePlayed;
	private LevelBridge bridge;
	public int nextLevel;
	private EventUtil util;


	public static LevelEvents1_1 FindMe() {
		return  GameObject.FindObjectOfType<LevelEvents1_1>();
	}

	void Awake() {

		bridge = LevelBridge.FindMe ();
		util = EventUtil.FindMe ();
	}

	void Start() {
		leftController = util.getLeftController();
		rightController = util.getRightController();
		FunctionController leftFunc = leftController.GetComponent<FunctionController> ();
		FunctionController rightFunc = rightController.GetComponent<FunctionController> ();
		leftFunc.ChangeFunctionStatus (ControllerMode.Mode.Fist, false);
		leftFunc.ChangeFunctionStatus (ControllerMode.Mode.WebShot, false);
		leftFunc.ChangeFunctionStatus (ControllerMode.Mode.RetractShot, false);
		leftFunc.ChangeFunctionStatus (ControllerMode.Mode.Rope, false);
		rightFunc.ChangeFunctionStatus (ControllerMode.Mode.Fist, false);
		rightFunc.ChangeFunctionStatus (ControllerMode.Mode.WebShot, false);
		rightFunc.ChangeFunctionStatus (ControllerMode.Mode.RetractShot, false);
		rightFunc.ChangeFunctionStatus (ControllerMode.Mode.Rope, false);
		bridge.newLevel = nextLevel;
		util.GetWindowControllerFromWindow (message).updateArray();
		messagePlayed = false;
		message.SetActive (false);
	}

	public void showMessage() {
		if (!messagePlayed) {
			messagePlayed = true;
			message.SetActive (true);
			//message.GetComponent<Animator>().Play ("SignBlinkIn");
//			message.GetComponent<AudioSource> ().Play ();
		}
	}



}
