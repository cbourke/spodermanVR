using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorisBehavior : MonoBehaviour {

	public GameObject face;
	private Texture ouchFace;
	public GameObject speechBubble;
	public AudioClip ouchie;
	public bool punched;
	public float speed;
	public GameObject sightColl;
	private Texture talkFace;
	private bool walking = false;
	private EventUtil util;
	public bool colliding;

	void Awake() {
		util = EventUtil.FindMe ();
		ouchFace = (Texture)Resources.Load ("Textures/level1.2/ouchFace");
		ouchie = (AudioClip)Resources.Load ("Audio/Speech/ouch");
		talkFace = (Texture)Resources.Load ("Textures/level1.2/normalFace_talk");
		punched = false;
	}

	// Use this for initialization
	void Start () {
		speechBubble.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		if (util.visibleObj && util.visibleObj.GetInstanceID () == sightColl.GetInstanceID () && !walking) {
			walking = true;
			Destroy (sightColl);
			StartCoroutine (talk());

		}
		if (!colliding && !punched)
			LookAt (util.headset);
	}
		

	void OnCollisionEnter(Collision coll)
	{
		if (coll.collider.gameObject.name == "Fist")
		{
			GetComponent<Renderer> ().material.mainTexture = ouchFace;
			GetComponent<AudioSource> ().clip = ouchie;
			GetComponent<AudioSource> ().Play ();
			punched = true;
			colliding = true;
		}


	}

	void OnTriggerEnter(Collider coll) {
		colliding = true;
	}

	void OnTriggerExit(Collider coll) {
		colliding = false;
	}

	private IEnumerator walk() {
		while (!punched) {
			while (!punched && (Vector3.Distance(util.headset.transform.position , this.gameObject.transform.position) >= 1f)) {
				this.gameObject.transform.position = Vector3.MoveTowards (gameObject.transform.position , util.headset.transform.position , speed * Time.deltaTime);
				yield return null;
			}
			yield return null;
		}
	}

	private IEnumerator talk() {
		string pitch = "low";
		int clipNum = Random.Range (1,4);
		string clipSelect = "Audio/Speech/" + clipNum.ToString () + pitch;
		Texture oldFace = face.GetComponent<Renderer> ().material.mainTexture;
		GetComponent<AudioSource> ().clip = (AudioClip)Resources.Load (clipSelect);
		face.GetComponent<Renderer> ().material.mainTexture = talkFace;
		speechBubble.SetActive (true);
		GetComponent<AudioSource> ().Play ();
		yield return new WaitForSeconds (1f);
		face.GetComponent<Renderer> ().material.mainTexture = oldFace;
		StartCoroutine (walk());
		yield return new WaitForSeconds (2f);
		speechBubble.SetActive (false);

	}

	private void LookAt(GameObject target) {
		transform.LookAt (util.headset.transform.position - new Vector3(0f , util.headset.transform.position.y - transform.position.y , 0f));
		//		Debug.DrawRay (transform.position , transform.forward , Color.red);
		//		Debug.DrawRay (transform.position , target.transform.position - transform.position , Color.blue);
	}

	//	public IEnumerator faceTalk(GameObject face , GameObject speechBubble , Texture faceTex , Texture text , string pitch) {
	//		speechBubble.SetActive (true);
	//		int clipNum = Random.Range (1,4);
	//		string clipSelect = "Audio/Speech/" + clipNum.ToString () + pitch;
	//		Texture oldFace = face.GetComponent<Renderer> ().material.mainTexture;
	//		face.GetComponent<AudioSource> ().clip = (AudioClip)Resources.Load (clipSelect);
	//		face.GetComponent<Renderer> ().material.mainTexture = faceTex;
	//		speechBubble.GetComponent<Renderer> ().material.mainTexture = text;
	//		face.GetComponent<AudioSource> ().Play ();
	//		yield return new WaitForSeconds (1);
	//		face.GetComponent<Renderer> ().material.mainTexture = oldFace;
	//		yield return new WaitForSeconds (GetComponent<LevelEvents1_2>().feedDelay);
	//		speechBubble.SetActive (false);
	//	}
}
