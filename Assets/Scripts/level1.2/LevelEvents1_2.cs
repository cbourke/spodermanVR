using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEvents1_2 : MonoBehaviour {

	public GameObject leftController;
	public GameObject rightController;
	public GameObject startLight;
	public GameObject feedA;
	public GameObject feedAText;
	public GameObject feedB;
	//public GameObject feedC;
	public float feedDelay;
	public GameObject feedBtext1;	//ropeshot diagram
	public GameObject feedBtext2;	//correct! image
	public GameObject feedBtext3;	//webshot diagram
	public GameObject feedBtext4;	//retract diagram
	public GameObject feedBtext5;	//fist diagram
	public GameObject ropeShotBlocks;
	public GameObject ropeShotLights;
	public GameObject speaker;
	public GameObject targetSetup;
	public GameObject target;
	public GameObject retractSetup;
	public GameObject retractCube;
	public GameObject enemySetup;
	public GameObject enemy;
	public GameObject enemyFace;
	public GameObject enemySpeechBubble;
	public GameObject exitDoor;
	public bool debugBool;


	private FunctionController leftFunc;
	private FunctionController rightFunc;
	private int currStage;
	public bool actionReady;
	private AudioClip textChange; 
	private AudioClip correct;
	private Texture feedAtext2;
	private GameObject nodeKeeper;
	private EventUtil util;



	void Awake() {
		leftFunc = leftController.GetComponent<FunctionController> ();
		rightFunc = rightController.GetComponent<FunctionController> ();
		feedAtext2 = (Texture)Resources.Load ("Textures/level1.2/lvl1_2feedA2");
		textChange = (AudioClip)Resources.Load ("Audio/windowAudio/textChange");
		correct = (AudioClip)Resources.Load ("Audio/General/correct!");
		feedBtext2.SetActive(false);
		feedA.SetActive (false);
		feedB.SetActive (false);
		//feedC.SetActive (false);
		actionReady = false;
		startLight.SetActive (false);
		debugBool = false;
		ropeShotBlocks.SetActive (false);
		ropeShotLights.SetActive (false);
		speaker.SetActive (false);
		feedBtext3.SetActive (false);
		targetSetup.SetActive (false);
		feedBtext4.SetActive (false);
		feedBtext5.SetActive (false);
		nodeKeeper = transform.parent.gameObject;
		util = this.GetComponent<EventUtil> ();
		currStage = 1;
		retractSetup.SetActive (false);
		enemySetup.SetActive (false);
		exitDoor.SetActive (false);
	}

	void Start() {
		leftFunc.fistEnabled = false;
		leftFunc.shotEnabled = false;
		leftFunc.retractEnabled = false;
		leftFunc.ropeEnabled = false;
		rightFunc.fistEnabled = false;
		rightFunc.shotEnabled = false;
		rightFunc.retractEnabled = false;
		rightFunc.ropeEnabled = false;
		StartCoroutine (cueScene());

	}

	void Update () {

		if (actionReady) {
			switch (currStage) {
			case 2: 
				if (leftFunc.currentMode.ToString() == "Rope" || rightFunc.currentMode.ToString() == "Rope") {
				//if (debugBool) {
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
				//if (debugBool) {
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
//				if (leftFunc.GetComponent<ControllerGrab> ().objectInHand != null || rightFunc.GetComponent<ControllerGrab> ().objectInHand != null) {
//					if (leftFunc.GetComponent<ControllerGrab> ().objectInHand.GetInstanceID () == retractCube.GetInstanceID ()
//						|| leftFunc.GetComponent<ControllerGrab> ().objectInHand.GetInstanceID () == retractCube.GetInstanceID ()) {
				if (!(Vector3.Distance(util.headset.transform.position , retractCube.transform.position) >= 1.5f)) {
						actionReady = false;
						StartCoroutine (cueSeventh());

				}
				if (debugBool) {
					actionReady = false;
					StartCoroutine (cueSeventh ());
				}
				break;

			case 8:
				if (Vector3.Distance(retractCube.transform.position , leftFunc.gameObject.transform.position) >= 4.0f) {
					actionReady = false;
					StartCoroutine (cueEighth ());
				}
				break;

			case 9:
				if (leftFunc.currentMode.ToString() == "Fist" || rightFunc.currentMode.ToString() == "Fist") {
				//if (debugBool) {
					actionReady = false;
					StartCoroutine (cueNinth ());
				}
				break;

			case 10:
				//if (Vector3.Distance(GetComponent<EventUtil>().headset.transform.position , enemy.transform.position) >= 3.0f) {
				//if (debugBool) {
				if (enemy.GetComponent<BorisHit>().punched) {
					actionReady = false;
					StartCoroutine (cueTenth ());
				}
				break;

			default:
				break;
			}
		}
	}

	private IEnumerator cueScene() {	//starts scene, looks for ropeMode
		yield return new WaitForSeconds (2);
		startLight.SetActive (true);
		startLight.GetComponent<AudioSource> ().Play ();
		yield return new WaitForSeconds (feedDelay);
		feedA.SetActive (true);
		StartCoroutine(gameObject.GetComponent<EventUtil> ().lookingAtCounter(feedAText));
		yield return new WaitUntil(() => GetComponent<EventUtil>().lookingBool == true);
		yield return new WaitForSeconds (feedDelay);
		util.changeTex (feedA , feedAText , feedAtext2);
		yield return new WaitForSeconds (feedDelay);
		feedA.GetComponent<Animator> ().SetTrigger ("FloatLeft");
		feedB.SetActive (true);
		speaker.SetActive (true);
		yield return new WaitForSeconds (1.1f);
		feedA.GetComponent<Animator> ().SetTrigger ("FloatRightHalf");
		feedB.GetComponent<Animator> ().SetTrigger ("FloatRightHalf");
		yield return new WaitForSeconds (feedDelay);
		leftFunc.ropeEnabled = true;
		rightFunc.ropeEnabled = true;
		currStage = 2;
		actionReady = true;
		enemySetup.SetActive (false);
	}



	private IEnumerator cueSecond() {	//player enters ropeMode, looks for rope
		feedBtext1.SetActive (false);
		feedBtext2.SetActive (true);
		util.playClip (feedB , correct);
		yield return new WaitForSeconds (feedDelay);
		util.changeTex (feedA,feedAText,(Texture)Resources.Load("Textures/level1.2/lvl1_2feedA3"));
		StartCoroutine(gameObject.GetComponent<EventUtil> ().lookingAtCounter(feedAText));
		yield return new WaitUntil(() => GetComponent<EventUtil>().lookingBool == true);
		yield return new WaitForSeconds (feedDelay);
		ropeShotBlocks.SetActive (true);
		ropeShotLights.SetActive (true);
		yield return new WaitForSeconds (feedDelay);
		feedBtext1.SetActive (true);
		feedBtext2.SetActive (false);
		util.playClip (feedB , textChange);
        //yield return new WaitForSeconds(feedDelay);
		currStage = 3;
		actionReady = true;
	}

	private IEnumerator cueThird() {	//player creates rope , looks for shotMode
		util.playClip (ropeShotBlocks , correct);
		feedBtext1.SetActive (false);
		feedBtext2.SetActive (true);
		util.playClip (feedB , textChange);
		yield return new WaitForSeconds(feedDelay);
		util.changeTex (feedA , feedAText , (Texture)Resources.Load("Textures/level1.2/lvl1_2feedA4"));
		StartCoroutine(gameObject.GetComponent<EventUtil> ().lookingAtCounter(feedAText));
		yield return new WaitUntil(() => GetComponent<EventUtil>().lookingBool == true);
		yield return new WaitForSeconds(feedDelay);
		ropeShotBlocks.SetActive (false);
		ropeShotLights.SetActive (false);
		nodeKeeper.GetComponent<WorldRopeNodeTracker> ().deleteRopes ();
		feedBtext3.SetActive (true);
		feedBtext2.SetActive (false);
		feedB.GetComponent<AudioSource> ().clip = textChange;
		feedB.GetComponent<AudioSource> ().Play ();
		//yield return new WaitForSeconds (feedDelay);
		leftFunc.shotEnabled = true;
		rightFunc.shotEnabled = true;
		currStage = 4;
		actionReady = true;
	}

	private IEnumerator cueFourth() {	//player enters shotMode , looks for target shot
		feedBtext3.SetActive (false);
		feedBtext2.SetActive (true);
		util.changeTex (feedB , feedBtext2 , (Texture)Resources.Load("Textures/level1.2/lvl1_2feedBshoot"));
		util.playClip (feedB , correct);
		yield return new WaitForSeconds (feedDelay);
		util.changeTex (feedA , feedAText , (Texture)Resources.Load("Textures/level1.2/lvl1_2feedA5"));
		StartCoroutine(gameObject.GetComponent<EventUtil> ().lookingAtCounter(feedAText));
		yield return new WaitUntil(() => GetComponent<EventUtil>().lookingBool == true);
		yield return new WaitForSeconds (feedDelay);
		targetSetup.SetActive (true);
		targetSetup.GetComponent<AudioSource>().Play ();
		feedBtext3.SetActive (true);
		feedBtext2.SetActive (false);
		util.playClip (feedB , textChange);
		//yield return new WaitForSeconds (feedDelay);
		currStage = 5;
		actionReady = true;
	}

	private IEnumerator cueFifth() {	//player shoots target , looks for retractModed
		util.playClip (targetSetup , correct);
		feedBtext3.SetActive (false);
		feedBtext2.SetActive (true);
		util.changeTex (feedB , feedBtext2 , (Texture)Resources.Load("Textures/level1.2/lvl1_2feedBshoot"));
		yield return new WaitForSeconds (feedDelay);
		util.changeTex (feedA , feedAText , (Texture)Resources.Load("Textures/level1.2/lvl1_2feedA6"));
		StartCoroutine(gameObject.GetComponent<EventUtil> ().lookingAtCounter(feedAText));
		yield return new WaitUntil(() => GetComponent<EventUtil>().lookingBool == true);
		yield return new WaitForSeconds (feedDelay);
		targetSetup.SetActive (false);
		util.changeTex (feedA , feedAText , (Texture)Resources.Load("Textures/level1.2/lvl1_2feedA7"));
		yield return new WaitForSeconds (feedDelay);
		feedBtext2.SetActive (false);
		feedBtext4.SetActive (true);
		feedB.GetComponent<AudioSource> ().clip = textChange;
		feedB.GetComponent<AudioSource> ().Play ();
		util.playClip (feedB , textChange);
		leftFunc.retractEnabled = true;
		rightFunc.retractEnabled = true;
		//yield return new WaitForSeconds (feedDelay);
		currStage = 6;
		actionReady = true;
	}

	private IEnumerator cueSixth() {	//player enters retractShot , looks for retract the cube
		
		feedBtext4.SetActive (false);
		feedBtext2.SetActive (true);
		util.changeTex (feedB , feedBtext2 , (Texture)Resources.Load("Textures/level1.2/lvl1_2feedBretract"));
		util.playClip(feedB , correct);
		yield return null;
		yield return new WaitForSeconds (feedDelay);
		util.changeTex (feedA , feedAText , (Texture)Resources.Load("Textures/level1.2/lvl1_2feedA8"));
		StartCoroutine(gameObject.GetComponent<EventUtil> ().lookingAtCounter(feedAText));
		yield return new WaitUntil(() => GetComponent<EventUtil>().lookingBool == true);
		yield return new WaitForSeconds (feedDelay);
		util.changeTex (feedA , feedAText , (Texture)Resources.Load("Textures/level1.2/lvl1_2feedA9"));
		yield return new WaitForSeconds (feedDelay);
		retractSetup.SetActive (true);
		feedBtext2.SetActive (false);
		feedBtext4.SetActive (true);
		util.playClip (feedB , textChange);
		//yield return new WaitForSeconds (feedDelay);
		currStage = 7;
		actionReady = true;
	}

	private IEnumerator cueSeventh() {	//player retracts the cube , looks for cube to be away from area
		feedBtext4.SetActive (false);
		feedBtext2.SetActive (true);
		util.playClip (feedB , correct);
		yield return new WaitForSeconds (feedDelay);
		util.changeTex (feedA , feedAText , (Texture)Resources.Load("Textures/level1.2/lvl1_2feedA10"));
		//yield return new WaitForSeconds (feedDelay);
		currStage = 8;
		actionReady = true;
	}

	private IEnumerator cueEighth() {	//player throws cube , looks for fistMode
		yield return new WaitForSeconds (feedDelay);
		util.changeTex(feedA , feedAText , (Texture)Resources.Load("Textures/level1.2/lvl1_2feedA11"));
		StartCoroutine(gameObject.GetComponent<EventUtil> ().lookingAtCounter(feedAText));
		yield return new WaitUntil(() => GetComponent<EventUtil>().lookingBool == true);
		yield return new WaitForSeconds (feedDelay);
		retractSetup.SetActive (false);
		feedBtext2.SetActive (false);
		feedBtext5.SetActive (true);
		util.playClip (feedB , textChange);
		//yield return new WaitForSeconds (feedDelay);
		leftFunc.fistEnabled = true;
		rightFunc.fistEnabled = true;
		currStage = 9;
		actionReady = true;
	}

	private IEnumerator cueNinth() {	//player in fistMode , looks for person punch
		feedBtext2.SetActive(true);
		feedBtext5.SetActive (false);
		util.changeTex (feedB , feedBtext2 , (Texture)Resources.Load("Textures/level1.2/lvl1_2feedBfist"));
		util.playClip (feedB , correct);
		yield return new WaitForSeconds (feedDelay);
		util.changeTex (feedA , feedAText , (Texture)Resources.Load("Textures/level1.2/lvl1_2feedA12"));
		StartCoroutine(gameObject.GetComponent<EventUtil> ().lookingAtCounter(feedAText));
		yield return new WaitUntil(() => GetComponent<EventUtil>().lookingBool == true);
		retractSetup.SetActive (false);
		yield return new WaitForSeconds (feedDelay);
		enemySetup.SetActive (true);
		enemySpeechBubble.SetActive (false);
		StartCoroutine(gameObject.GetComponent<EventUtil> ().lookingAtCounter(enemy));
		yield return new WaitUntil(() => GetComponent<EventUtil>().lookingBool == true);
		yield return new WaitForSeconds (1);
		StartCoroutine(util.faceTalk (enemyFace , enemySpeechBubble , (Texture)Resources.Load("Textures/level1.2/normalFace_talk") , (Texture)Resources.Load("Textures/level1.2/borisSpeech1") , "Low"));
		//yield return new WaitForSeconds (1);
		while (Vector3.Distance(util.headset.transform.position , enemySetup.transform.position) >= 1f) {
			enemySetup.transform.position = Vector3.MoveTowards (enemySetup.transform.position , util.headset.transform.position , 1.1f*Time.deltaTime);
			yield return null;
		}
		feedBtext2.SetActive(false);
		feedBtext5.SetActive (true);
		util.playClip (feedB , textChange);
		yield return new WaitForSeconds (feedDelay);
		currStage = 10;
		actionReady = true;
	}

	private IEnumerator cueTenth() {	//player has punched Boris , player ends tutorial
		feedBtext2.SetActive(true);
		feedBtext5.SetActive (false);
		util.playClip (feedB , textChange);
		yield return new WaitForSeconds (feedDelay);
		util.changeTex (feedA , feedAText , (Texture)Resources.Load("Textures/level1.2/lvl1_2feedA13"));
		StartCoroutine(gameObject.GetComponent<EventUtil> ().lookingAtCounter(feedAText));
		yield return new WaitUntil(() => GetComponent<EventUtil>().lookingBool == true);
		yield return new WaitForSeconds (feedDelay);
		util.changeTex (feedA , feedAText , (Texture)Resources.Load("Textures/level1.2/lvl1_2feedA14"));
		yield return new WaitForSeconds (feedDelay);
		exitDoor.SetActive (true);
		Vector3 doorPos = new Vector3 (util.headset.transform.forward.x , util.headset.transform.position.y ,  util.headset.transform.forward.z);
		exitDoor.transform.position = util.headset.transform.position - doorPos*1f;
		exitDoor.transform.position = exitDoor.transform.position + new Vector3 (0f,0.75f,0f);
		exitDoor.transform.LookAt (new Vector3(util.headset.transform.position.x , 0.75f , util.headset.transform.position.z));
		currStage = 11;
	}
		
}
