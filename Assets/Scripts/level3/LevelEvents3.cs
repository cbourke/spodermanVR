﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEvents3 : MonoBehaviour {


	public GameObject window1;
	public GameObject window1text;
	public GameObject greenLight;
	public GameObject window2;
	public GameObject window2text;
	public float feedDelay;

	private GameObject playerHead;
	private EventUtil util;
	private float lightIntensity;
	// Use this for initialization
	void Awake() {
		greenLight = GameObject.Find ("GreenLight1");
		lightIntensity = greenLight.GetComponent<Light> ().range;
		playerHead = GameObject.Find ("Camera (eye)");
		window1.SetActive (false);
		window2.SetActive (false);
		util = this.GetComponent<EventUtil> ();
	}

	void Start () {
		
		StartCoroutine (sceneStart());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private IEnumerator sceneStart() {
		yield return new WaitForSeconds (2.0f);
		window1.SetActive (true);
		yield return new WaitForSeconds (2.0f);
//		StartCoroutine(gameObject.GetComponent<EventUtil> ().lookingAtCounter(window1text));
//		yield return new WaitUntil(() => GetComponent<EventUtil>().lookingBool == true);
//		yield return new WaitForSeconds (feedDelay);
		util.changeTex (window1 , window1text , (Texture)Resources.Load("Textures/level3/signA_2"));
		yield return new WaitForSeconds (feedDelay);
		greenLight.GetComponent<Light>().color = Color.green;
		StartCoroutine (greenLightFlicker(greenLight.GetComponent<Light>()));
		util.playClip (greenLight , (AudioClip)Resources.Load("Audio/General/softCorrectSound"));

	}

	public IEnumerator triggerZone1 () {
		window1.GetComponent<Animator> ().SetTrigger ("TurnOff");
		greenLight.GetComponent<Light> ().color = Color.white;
		util.playClip (greenLight , (AudioClip)Resources.Load("Audio/General/softCorrectSound"));
		greenLight.GetComponent<Light> ().range = lightIntensity;
		yield return new WaitForSeconds (2.0f);
		window2.SetActive (true);
//		StartCoroutine(gameObject.GetComponent<EventUtil> ().lookingAtCounter(window2text));
//		yield return new WaitUntil(() => GetComponent<EventUtil>().lookingBool == true);
//		yield return new WaitForSeconds (feedDelay);
		util.changeTex(window2 , window2text , (Texture)Resources.Load("Textures/level3/signB_2"));
		yield return new WaitForSeconds (feedDelay);
		util.changeTex(window2 , window2text , (Texture)Resources.Load("Textures/level3/signB_3"));
		yield return new WaitForSeconds (feedDelay);
		greenLight = GameObject.Find ("GreenLight2");
	}

	private IEnumerator greenLightFlicker(Light lightSrc) {
		float top = lightSrc.range;
		while (lightSrc.color != Color.white) {
			lightSrc.range = Mathf.Sin (Time.time) * (lightSrc.range/2) + top;
			yield return new WaitForEndOfFrame ();
		}
	}
}
