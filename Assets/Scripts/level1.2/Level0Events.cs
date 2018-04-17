using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level0Events : MonoBehaviour {


	public GameObject startLight;
	public GameObject feedA;
	public GameObject feedB;
	public GameObject feedAanim;
	public GameObject feedBanim;
	private Animator feedAanimAnim;
	private Animator feedBanimAnim;
	public float feedDelay;
	public GameObject ropeShotGif;
	public GameObject webShotGif;
	public GameObject fistGif;
	public GameObject retractGif;
	public GameObject checkMark;
	public GameObject ropeShotBlocks;
	public GameObject ropeShotLights;
	public GameObject speaker;
	public GameObject targetSetup;
	public GameObject target;
	public GameObject retractSetup;
	public GameObject retractCube;
	public GameObject enemy;
	public GameObject exitDoor;
	public bool debugBool;
	public Texture[] feedATex;
	public Texture[] feedBTex;
	public GameObject exitHandle;

	public FunctionController leftFunc;
	public FunctionController rightFunc;
	private int currStage;
	public bool actionReady;
	private AudioClip correct;
	private GameObject nodeKeeper;
	private EventUtil util;
	private int feedBcurrInd;



	void Awake() {
		correct = (AudioClip)Resources.Load ("Audio/General/correct!");
		nodeKeeper = transform.parent.gameObject;
		util = this.GetComponent<EventUtil> ();
		leftFunc = util.getLeftController().GetComponent<FunctionController> ();
		rightFunc = util.getRightController().GetComponent<FunctionController> ();
	}

	void Start() {
		feedAanimAnim = feedAanim.GetComponent<Animator> ();
		feedBanimAnim = feedBanim.GetComponent<Animator> ();
		util.GetWindowControllerFromWindow (feedA).updateArray (feedATex);
		util.GetWindowControllerFromWindow (feedB).updateArray (feedBTex);
		feedA.SetActive (false);
		feedB.SetActive (false);
		checkMark.SetActive (false);
		actionReady = false;
		startLight.SetActive (false);
		debugBool = false;
		ropeShotBlocks.SetActive (false);
		ropeShotLights.SetActive (false);
		speaker.SetActive (false);

		targetSetup.SetActive (false);

		leftFunc.ChangeFunctionStatus(ControllerMode.Mode.Fist , false);
		leftFunc.ChangeFunctionStatus(ControllerMode.Mode.WebShot , false);
		leftFunc.ChangeFunctionStatus(ControllerMode.Mode.RetractShot , false);
		leftFunc.ChangeFunctionStatus(ControllerMode.Mode.Rope , false);

		rightFunc.ChangeFunctionStatus(ControllerMode.Mode.Fist , false);
		rightFunc.ChangeFunctionStatus(ControllerMode.Mode.WebShot , false);
		rightFunc.ChangeFunctionStatus(ControllerMode.Mode.RetractShot , false);
		rightFunc.ChangeFunctionStatus(ControllerMode.Mode.Rope , false);

		currStage = 1;
		retractSetup.SetActive (false);
		enemy.SetActive (false);
		exitDoor.SetActive (false);
		StartCoroutine (cueScene());

	}

	void Update () {
		if (feedB.activeSelf) {
			feedBcurrInd = util.GetWindowControllerFromWindow (feedB).currIndex;
			webShotGif.SetActive (feedBcurrInd == 1);
			ropeShotGif.SetActive (feedBcurrInd == 0);
			retractGif.SetActive (feedBcurrInd == 2);
			fistGif.SetActive (feedBcurrInd == 3);
		}

		if (actionReady) {
			switch (currStage) {
			case 2: 
				if (leftFunc.currentMode.ToString() == "Rope" || rightFunc.currentMode.ToString() == "Rope") {
					actionReady = false;
					StartCoroutine (cueSecond ());
				}
				break;
			
			case 3:
				List<GameObject> listRope = nodeKeeper.GetComponent<WorldRopeNodeTracker> ().getRopeKeeper ();
				if (listRope.Count > 5) {
					nodeKeeper.GetComponent<WorldRopeNodeTracker> ().deleteRopes ();
				} else {
					foreach (GameObject rope in listRope) {
						if (rope.transform.localScale.y > 1) {
							actionReady = false;
							StartCoroutine (cueThird ());
							return;
						}
					}
				}
				break;
			
			case 4:
				if (leftFunc.currentMode.ToString() == "WebShot" || rightFunc.currentMode.ToString() == "WebShot") {
					actionReady = false;
					StartCoroutine (cueFourth ());
				}
				break;

			case 5:
				if (!target.activeSelf ) {
					actionReady = false;
					StartCoroutine (cueFifth ());
				}
				break;

			case 6:
				if (leftFunc.currentMode.ToString() == "RetractShot" || rightFunc.currentMode.ToString() == "RetractShot") {
				//if (debugBool) {
					actionReady = false;
					StartCoroutine (cueSixth ());
				}
				break;

			case 7:
				if (Vector3.Distance(util.headset.transform.position , retractCube.transform.position) <= 1.5f) {
						actionReady = false;
						StartCoroutine (cueSeventh());

				}
				if (debugBool) {
					actionReady = false;
					StartCoroutine (cueSeventh ());
				}
				break;

			case 8:
				if (Vector3.Distance(retractCube.transform.position , util.headset.transform.position) >= 3f) {
					actionReady = false;
					StartCoroutine (cueEighth ());
				}
				break;

			case 9:
				if (leftFunc.currentMode.ToString() == "Fist" || rightFunc.currentMode.ToString() == "Fist") {
					actionReady = false;
					StartCoroutine (cueNinth ());
				}
				break;

			case 10:
				if (enemy.GetComponent<BorisBehavior>().punched) {
					actionReady = false;
					StartCoroutine (cueTenth ());
				}
				break;

			default:
				break;
			}
		}
	}

	private IEnumerator blinkCheckMark() {
		checkMark.SetActive (true);
		yield return new WaitForSeconds (3f);
		checkMark.SetActive (false);
	}

	private IEnumerator cueScene() {	//starts scene, looks for ropeMode
		yield return new WaitForSeconds (2);
		startLight.SetActive (true);
		startLight.GetComponent<AudioSource> ().Play ();
		yield return new WaitForSeconds (feedDelay);
		feedA.SetActive (true);
		util.GetWindowControllerFromWindow (feedA).ChangeLock (1);
		//set message lock
		yield return new WaitUntil (() => util.GetWindowControllerFromWindow (feedA).currIndex == util.GetWindowControllerFromWindow (feedA).msgLock);
		yield return new WaitForSeconds (feedDelay);
		feedAanimAnim.SetTrigger ("FloatLeft");
		feedB.SetActive (true);
		util.GetWindowControllerFromWindow (feedB).ChangeLock (0);
		speaker.SetActive (true);
		yield return new WaitForSeconds (1.1f);
		feedAanimAnim.SetTrigger ("FloatRightHalf");
		feedBanimAnim.SetTrigger ("FloatRightHalf");
		leftFunc.ChangeFunctionStatus(ControllerMode.Mode.Rope , true);
		rightFunc.ChangeFunctionStatus(ControllerMode.Mode.Rope , true);
		currStage = 2;
		actionReady = true;
		//enemySetup.SetActive (false);
	}



	private IEnumerator cueSecond() {	//player enters ropeMode, looks for rope
		StartCoroutine(blinkCheckMark());
		util.playClip (feedBanim , correct);
		yield return new WaitForSeconds (feedDelay);
		util.GetWindowControllerFromWindow (feedA).ChangeMsg (2);
		util.GetWindowControllerFromWindow (feedA).ChangeLock (3);
		yield return new WaitUntil (() => util.GetWindowControllerFromWindow (feedA).currIndex == util.GetWindowControllerFromWindow (feedA).msgLock);
		yield return new WaitForSeconds (feedDelay);
		ropeShotBlocks.SetActive (true);
		ropeShotLights.SetActive (true);
		currStage = 3;
		actionReady = true;
	}

	private IEnumerator cueThird() {	//player creates rope , looks for shotMode
		StartCoroutine(blinkCheckMark());
		util.playClip (feedBanim , correct);
		yield return new WaitForSeconds (feedDelay);
		util.GetWindowControllerFromWindow (feedA).ChangeMsg (4);
		util.GetWindowControllerFromWindow (feedA).ChangeLock (5);
		yield return new WaitUntil (() => util.GetWindowControllerFromWindow (feedA).currIndex == util.GetWindowControllerFromWindow (feedA).msgLock);
		yield return new WaitForSeconds (feedDelay);
		util.GetWindowControllerFromWindow (feedB).ChangeMsg (1);
		util.GetWindowControllerFromWindow (feedB).ChangeLock (1);
		leftFunc.ChangeFunctionStatus (ControllerMode.Mode.WebShot , true);
		rightFunc.ChangeFunctionStatus (ControllerMode.Mode.WebShot , true);
		currStage = 4;
		actionReady = true;
	}

	private IEnumerator cueFourth() {	//player enters shotMode , looks for target shot
		StartCoroutine(blinkCheckMark());
		util.playClip (feedBanim , correct);
		yield return new WaitForSeconds (feedDelay);
		util.GetWindowControllerFromWindow (feedA).ChangeMsg (6);
		util.GetWindowControllerFromWindow (feedA).ChangeLock (7);
		yield return new WaitUntil (() => util.GetWindowControllerFromWindow (feedA).currIndex == util.GetWindowControllerFromWindow (feedA).msgLock);
		ropeShotBlocks.SetActive (false);
		nodeKeeper.GetComponent<WorldRopeNodeTracker> ().deleteRopes ();
		yield return new WaitForSeconds (feedDelay);
		targetSetup.SetActive (true);
		currStage = 5;
		actionReady = true;
	}

	private IEnumerator cueFifth() {	//player shoots target , looks for retractModed
		StartCoroutine(blinkCheckMark());
		util.playClip (feedBanim , correct);
		targetSetup.SetActive (false);
		yield return new WaitForSeconds (feedDelay);
		util.GetWindowControllerFromWindow (feedA).ChangeMsg (8);
		util.GetWindowControllerFromWindow (feedA).ChangeLock (9);
		yield return new WaitUntil (() => util.GetWindowControllerFromWindow (feedA).currIndex == util.GetWindowControllerFromWindow (feedA).msgLock);
		yield return new WaitForSeconds (feedDelay);
		util.GetWindowControllerFromWindow (feedB).ChangeMsg (2);
		util.GetWindowControllerFromWindow (feedB).ChangeLock (2);
		leftFunc.ChangeFunctionStatus (ControllerMode.Mode.RetractShot , true);
		rightFunc.ChangeFunctionStatus (ControllerMode.Mode.RetractShot , true);
		currStage = 6;
		actionReady = true;
	}

	private IEnumerator cueSixth() {	//player enters retractShot , looks for retract the cube
		StartCoroutine(blinkCheckMark());
		util.playClip (feedBanim , correct);
		yield return new WaitForSeconds (feedDelay);
		util.GetWindowControllerFromWindow (feedA).ChangeMsg (10);
		util.GetWindowControllerFromWindow (feedA).ChangeLock (10);
		yield return new WaitForSeconds (feedDelay);
		retractSetup.SetActive (true);
		currStage = 7;
		actionReady = true;
	}

	private IEnumerator cueSeventh() {	//player retracts the cube , looks for cube to be away from area
		StartCoroutine(blinkCheckMark());
		util.playClip (feedBanim , correct);
		yield return new WaitForSeconds (feedDelay);
		util.GetWindowControllerFromWindow (feedA).ChangeMsg (11);
		util.GetWindowControllerFromWindow (feedA).ChangeLock (12);
		currStage = 8;
		actionReady = true;
	}

	private IEnumerator cueEighth() {	//player throws cube , looks for fistMode
		StartCoroutine(blinkCheckMark());
		util.playClip (feedBanim , correct);
		yield return new WaitForSeconds (feedDelay);
		util.GetWindowControllerFromWindow (feedA).ChangeMsg (13);
		util.GetWindowControllerFromWindow (feedA).ChangeLock (13);
		util.GetWindowControllerFromWindow (feedB).ChangeMsg (3);
		util.GetWindowControllerFromWindow (feedB).ChangeLock (3);
		leftFunc.ChangeFunctionStatus (ControllerMode.Mode.Fist , true);
		rightFunc.ChangeFunctionStatus (ControllerMode.Mode.Fist , true);
		currStage = 9;
		actionReady = true;
	}

	private IEnumerator cueNinth() {	//player in fistMode , looks for person punch
		StartCoroutine(blinkCheckMark());
		util.playClip (feedBanim , correct);
		yield return new WaitForSeconds (feedDelay);
		util.GetWindowControllerFromWindow (feedA).ChangeMsg (14);
		util.GetWindowControllerFromWindow (feedA).ChangeLock (15);
		yield return new WaitUntil (() => util.GetWindowControllerFromWindow (feedA).currIndex == util.GetWindowControllerFromWindow (feedA).msgLock);
		yield return new WaitForSeconds (feedDelay);
		retractSetup.SetActive (false);
		enemy.SetActive (true);
		currStage = 10;
		actionReady = true;
	}

	private IEnumerator cueTenth() {	//player has punched Boris , player ends tutorial
		StartCoroutine(blinkCheckMark());
		util.playClip (feedBanim , correct);
		yield return new WaitForSeconds (feedDelay);
		util.GetWindowControllerFromWindow (feedA).ChangeMsg (16);
		util.GetWindowControllerFromWindow (feedA).ChangeLock (19);
		yield return new WaitUntil (() => util.GetWindowControllerFromWindow (feedA).currIndex == util.GetWindowControllerFromWindow (feedA).msgLock);
		yield return new WaitForSeconds (feedDelay);
		exitDoor.SetActive (true);
		Vector3 doorPos = new Vector3 (util.headset.transform.forward.x , util.headset.transform.position.y ,  util.headset.transform.forward.z);
		exitDoor.transform.position = util.headset.transform.position - doorPos * 1f;
		exitDoor.transform.position += new Vector3 (0f,1f,0f);
		exitDoor.transform.LookAt (new Vector3(util.headset.transform.position.x , 1f , util.headset.transform.position.z));
		exitHandle.GetComponent<LevelBridge> ().newLevel = 3;
		currStage = 20;

	}
		
}
