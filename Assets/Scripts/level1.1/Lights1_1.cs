using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lights1_1 : MonoBehaviour {

	public GameObject startMessage;
	public GameObject start;
	public GameObject main;
	public GameObject diagram;
	public float triggerDelay;
	public GameObject climbSign;
	public GameObject speaker;
	public GameObject blocksParent;
	public GameObject startMessageText;
	public bool debugBool;

	private AudioSource startSource;
	private AudioSource diagramSource;

	void Awake () {
		startSource = start.GetComponent<AudioSource> ();
		diagramSource = diagram.GetComponent<AudioSource> ();
		start.SetActive (false);
		main.SetActive (false);
		diagram.SetActive (false);
		blocksParent.SetActive (false);
		climbSign.SetActive (false);
		startMessage.SetActive (false);

	}
	// Use this for initialization
	void Start () {
		
		StartCoroutine (cueLights());

	}

	private IEnumerator cueLights() {
		yield return new WaitForSeconds (triggerDelay);
		startMessage.SetActive (true);
		startMessage.GetComponent<AudioSource> ().Play ();
		StartCoroutine(gameObject.GetComponent<EventUtil> ().lookingAtCounter(startMessageText));
		yield return new WaitUntil(() => GetComponent<EventUtil>().lookingBool == true);
		yield return new WaitForSeconds (triggerDelay);
		startMessage.GetComponent<AudioSource> ().Play ();
		GetComponent<EventUtil> ().changeTex (startMessage , startMessageText , (Texture)Resources.Load ("Textures/level1.1/welcome2"));
		yield return new WaitForSeconds (triggerDelay);
		startMessage.GetComponent<Animator> ().SetTrigger ("TurnOff");
		yield return new WaitForSeconds (triggerDelay);
		startMessage.SetActive (false);
		yield return new WaitForSeconds (triggerDelay);
		start.SetActive (true);
		startSource.Play ();
		yield return new WaitForSeconds (triggerDelay);
		diagram.SetActive (true);
		diagramSource.Play ();
		yield return new WaitForSeconds (triggerDelay);
		main.SetActive (true);
		startSource.Play ();
		yield return new WaitForSeconds (triggerDelay);
		climbSign.SetActive (true);
		climbSign.GetComponent<AudioSource> ().Play ();
		blocksParent.SetActive (true);
		Animator[] lister = blocksParent.GetComponentsInChildren<Animator> ();
		foreach(Animator block in lister) {
			block.GetComponent<Animator> ().Play ("grabBlinkIn");
		}
		speaker.GetComponent<AudioSource> ().Play ();

	}

}
