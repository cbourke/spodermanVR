using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEvents3 : MonoBehaviour {


	public GameObject window1;
//	public GameObject window1text;
	public GameObject greenLight;
	public GameObject window2;
	public GameObject window3;
//	public GameObject window2text;
	public float feedDelay;
	public LevelBridge levelBridge;
	public Texture[] window1Tex;
	public Texture[] window2Tex;
	public Texture[] window3Tex;
	public GameObject window2gif;
	public GameObject window3gif;
	public GameObject speaker;
	private EventUtil util;
	private float lightIntensity;

	public static LevelEvents3 FindMe() {
		return  GameObject.FindObjectOfType<LevelEvents3>();
	}

	// Use this for initialization
	void Awake() {
//		greenLight = GameObject.Find ("GreenLight1");
//		lightIntensity = greenLight.GetComponent<Light> ().range;
		util = this.GetComponent<EventUtil> ();
	}

	void Start () {
		window2.SetActive (false);
		window3.SetActive (false);
		levelBridge = LevelBridge.FindMe ();
		levelBridge.newLevel = 4;
		speaker.SetActive (false);
		StartCoroutine (sceneStart());

		util.GetWindowControllerFromWindow (window1).updateArray (window1Tex);
		util.GetWindowControllerFromWindow (window2).updateArray (window2Tex);
		util.GetWindowControllerFromWindow (window3).updateArray (window3Tex);
	}

	// Update is called once per frame
	void Update () {
		window2gif.SetActive (util.GetWindowControllerFromWindow (window2).currIndex == 3);
		window3gif.SetActive (util.GetWindowControllerFromWindow (window3).currIndex == 3);

	}

	private IEnumerator sceneStart() {
//		
////		yield return new WaitForSeconds (feedDelay);
////		window1.SetActive (true);
////		StartCoroutine(gameObject.GetComponent<EventUtil> ().lookingAtCounter(window1text));
////		yield return new WaitUntil(() => GetComponent<EventUtil>().lookingBool == true);
////		yield return new WaitForSeconds (feedDelay * 1.25f);
////		util.changeTex (window1 , window1text , (Texture)Resources.Load("Textures/level3/signA_2"));
////		yield return new WaitForSeconds (feedDelay);
////		greenLight.GetComponent<Light>().color = Color.green;
////		StartCoroutine (greenLightFlicker(greenLight.GetComponent<Light>()));
////		util.playClip (greenLight , (AudioClip)Resources.Load("Audio/General/softCorrectSound"));
		yield return new WaitForSeconds(5f);
		speaker.SetActive (true);
	}

	public void triggerZone1 () {
//		StopCoroutine (sceneStart());
//		window1.GetComponent<Animator> ().SetTrigger ("TurnOff");
//		greenLight.GetComponent<Light> ().color = Color.white;
//		util.playClip (greenLight , (AudioClip)Resources.Load("Audio/General/softCorrectSound"));
//		greenLight.GetComponent<Light> ().range = lightIntensity;
//		yield return new WaitForSeconds (feedDelay);
//		window2.SetActive (true);
////		StartCoroutine(gameObject.GetComponent<EventUtil> ().lookingAtCounter(window2text));
//		yield return new WaitUntil(() => GetComponent<EventUtil>().lookingBool == true);
//		yield return new WaitForSeconds (feedDelay * 1.25f);
////		util.changeTex(window2 , window2text , (Texture)Resources.Load("Textures/level3/signB_2"));
//		yield return new WaitForSeconds (feedDelay);
////		util.changeTex(window2 , window2text , (Texture)Resources.Load("Textures/level3/signB_3"));
//		yield return new WaitForSeconds (feedDelay);
//		greenLight = GameObject.Find ("GreenLight2");
//		greenLight.GetComponent<Light> ().color = Color.green;
//		StartCoroutine(greenLightFlicker(greenLight.GetComponent<Light>()));
		window2.SetActive(true);

	}

	public void triggerZone2 () {
//		StopCoroutine (triggerZone1());
//		greenLight = GameObject.Find ("GreenLight2");
//		window2.GetComponent<Animator> ().SetTrigger ("TurnOff");
//		greenLight.GetComponent<Light> ().color = Color.white;
//		util.playClip (greenLight , (AudioClip)Resources.Load("Audio/General/softCorrectSound"));
		window3.SetActive(true);
	}

//	public void triggerZone3 () {
////		greenLight = GameObject.Find ("GreenLight3");
////		util.playClip (greenLight , (AudioClip)Resources.Load("Audio/General/softCorrectSound"));
////		greenLight.GetComponent<Light> ().color = Color.green;
////		StartCoroutine(greenLightFlicker(greenLight.GetComponent<Light>()));
//	}

//	private IEnumerator greenLightFlicker(Light lightSrc) {
//		float top = lightSrc.range;
//		while (lightSrc.color.r >= 0) {
//			lightSrc.range = Mathf.Sin (Time.time) * (lightSrc.range/2) + top;
//			yield return new WaitForEndOfFrame ();
//		}
//	}
}
