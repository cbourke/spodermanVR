using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEvents3 : MonoBehaviour {


	public GameObject window1;
	public GameObject window1text;
	public GameObject greenLight;
	public float feedDelay;

	private GameObject playerHead;
	private EventUtil util;
	// Use this for initialization
	void Awake() {
		playerHead = GameObject.Find ("Camera (eye)");
		window1.SetActive (false);
		util = this.GetComponent<EventUtil> ();
	}

	void Start () {
		greenLight = GameObject.Find ("GreenLight1");
		greenLight.SetActive (false);
		StartCoroutine (sceneStart());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private IEnumerator sceneStart() {
		yield return new WaitForSeconds (2.0f);
		window1.SetActive (true);
		StartCoroutine(gameObject.GetComponent<EventUtil> ().lookingAtCounter(window1text));
		yield return new WaitUntil(() => GetComponent<EventUtil>().lookingBool == true);
		yield return new WaitForSeconds (feedDelay);
		util.changeTex (window1 , window1text , (Texture)Resources.Load("Textures/level3/signA_2"));



	}

	public void triggerZone1 () {
		greenLight.GetComponent<Light> ().color = Color.white;
		util.playClip (greenLight , (AudioClip)Resources.Load("Audio/General/softCorrectSound"));

	}
}
