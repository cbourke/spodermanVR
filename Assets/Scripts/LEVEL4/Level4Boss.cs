using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level4Boss : MonoBehaviour {

	public EventUtil util;
	public GameObject playerHead;
	public AudioClip laugh;
	public AudioClip bossMusic;
	public AudioClip bossDefeat;
	public AudioClip fanfare;
	public bool seen = false;
	public GameObject textWindow;
	public GameObject talkMouth;
	public GameObject smashMouth;
	public Texture[] speechTextures;
	public List<Vector2> spawnPos;
	public bool defeated = false;
	private AudioSource audioS;
	private GameObject badGuy;
	public bool activate = false;

	public static Level4Boss FindMe() {
		return  GameObject.FindObjectOfType<Level4Boss>();
	}

	// Use this for initialization
	void Awake() {
		util = EventUtil.FindMe ();
		playerHead = HeadColliderHandler.FindMe ().gameObject;
	}

	void Start () {
		laugh = (AudioClip)Resources.Load ("Audio/General/evilLaugh");
		bossMusic = (AudioClip)Resources.Load ("Audio/LEVEL4/ff7Boss");
		bossDefeat = (AudioClip)Resources.Load ("Audio/LEVEL4/bossDefeat");
		fanfare = (AudioClip)Resources.Load ("Audio/LEVEL4/ff7Victory");
		badGuy = (GameObject)Resources.Load ("Prefabs/BadGuy");
		audioS = GetComponent<AudioSource> ();
		smashMouth.SetActive (false);
		foreach (Transform child in transform) {
			if (child.gameObject.name == "Marker") {
				spawnPos.Add (new Vector2 (child.transform.position.x, child.transform.position.z));
				child.gameObject.SetActive (false);
			}
		}
		util.GetWindowControllerFromWindow (textWindow).updateArray(speechTextures , false);
		util.GetWindowControllerFromWindow (textWindow).ChangeLock (1);
		textWindow.SetActive (false);
		GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
	}

	public void OnTriggerEnter(Collider other) {
		if (other.gameObject.name == "WebShot") {
			util.GetWindowControllerFromWindow (textWindow).ChangeLock(3);
			util.GetWindowControllerFromWindow (textWindow).ChangeMsg (2);
		
		}
	}
	
	// Update is called once per frame
	void Update () {
		Quaternion q = Quaternion.LookRotation (playerHead.transform.position - transform.position);
		transform.rotation = Quaternion.RotateTowards (transform.rotation , q , 100 * Time.deltaTime);
//		if (util.FieldOfVision(this.gameObject , 0.2f , false)) {
		if (activate) {
			if (!seen) {
				seen = true;
				StartCoroutine (spawnMinions());
				textWindow.SetActive (true);
				audioS.clip = laugh;
				audioS.Play ();
				StartCoroutine (cueMusic(false));
			}
		}
	}


	private IEnumerator cueMusic(bool victory) {
		yield return new WaitForSeconds (1.7f);
		if (victory) {
			audioS.clip = fanfare;
			audioS.loop = false;
		}
		else {
			audioS.clip = bossMusic;
			audioS.loop = true;
		}

		audioS.Play ();
	}

	private IEnumerator spawnMinions() {
		while (!defeated) {
			int rand = Random.Range (0,3);
			GameObject badTemp = GameObject.Instantiate (badGuy);
			badTemp.AddComponent<BossPuncher> ();
			badTemp.transform.position = new Vector3 (spawnPos[rand].x , 14.8f , spawnPos[rand].y);
			yield return new WaitForSeconds(5f);
		}
	}

	private IEnumerator endScene() {
		yield return new WaitForSeconds (15f);
		UnityEngine.SceneManagement.SceneManager.LoadScene (0);
	}

	public void Death() {
		GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;
		util.GetWindowControllerFromWindow (textWindow).ChangeLock(int.MaxValue);
		util.GetWindowControllerFromWindow (textWindow).ChangeMsg (4);
		defeated = true;
		talkMouth.SetActive (false);
		smashMouth.SetActive (true);
		audioS.loop = false;
		audioS.Stop ();
		audioS.clip = bossDefeat;
		audioS.Play ();
		StartCoroutine (cueMusic(true));
		StartCoroutine (endScene());
	}
}
