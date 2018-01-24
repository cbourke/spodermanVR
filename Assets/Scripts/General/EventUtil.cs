using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventUtil : MonoBehaviour {

	public GameObject headset;
	public GameObject visibleObj;
	public bool lookingBool;
	public float lookRecognitionTime;
	public bool talking;

	private AudioClip sound;
	private AudioClip speechSound;

	private int layerMask;

	void Awake() {
		headset = GameObject.Find ("Camera (eye)");
		layerMask = 1 << 8;
		layerMask = ~layerMask;
	}


	// Update is called once per frame
	void Update () {
		RaycastHit hit;	
		if (Physics.Raycast (headset.transform.position, headset.transform.forward, out hit, 100, layerMask)) {
			visibleObj = hit.collider.gameObject;
		} else {
			visibleObj = null;
		}
	}

	public GameObject getObject() {
		return visibleObj;
	}

	public bool lookingAtObj(GameObject obj) {
		if (visibleObj && obj && visibleObj.GetInstanceID () == obj.GetInstanceID ()) {
			return true;
		} else
			return false;
	}

	public IEnumerator lookingAtCounter(GameObject obj) {
		float timer = 0f;
		while (timer <= lookRecognitionTime /*&& !this.GetComponent<EventUtil>().lookingAtObj (feedAText)*/) {
			if (lookingAtObj (obj)) {
				timer += 0.25f;
				Debug.Log ("Still in loop...." + obj.name);
				yield return new WaitForSeconds(0.25f);
				if (timer >= lookRecognitionTime) {
					Debug.Log ("I saw it");
					lookingBool = true;
					yield return new WaitForSeconds(0.25f);
					lookingBool = false;
					yield break;
				}
				yield return null;
			}
			else {
				timer = 0f;
				yield return null;
			}
		}
		yield return null;

	}


	public void changeTex(GameObject obj , GameObject textObj , Texture tex) {
		sound = (AudioClip)Resources.Load ("Audio/windowAudio/textChange");
		obj.GetComponent<AudioSource> ().clip = sound;
		textObj.GetComponent<Renderer>().material.mainTexture = tex;
		obj.GetComponent<AudioSource> ().Play ();
	}

	public IEnumerator faceTalk(GameObject face , GameObject speechBubble , Texture faceTex , Texture text , string pitch) {
		speechBubble.SetActive (true);
		int clipNum = Random.Range (1,4);
		string clipSelect = "Audio/Speech/" + clipNum.ToString () + pitch;
		Texture oldFace = face.GetComponent<Renderer> ().material.mainTexture;
		face.GetComponent<AudioSource> ().clip = (AudioClip)Resources.Load (clipSelect);
		face.GetComponent<Renderer> ().material.mainTexture = faceTex;
		speechBubble.GetComponent<Renderer> ().material.mainTexture = text;
		face.GetComponent<AudioSource> ().Play ();
		yield return new WaitForSeconds (1);
		face.GetComponent<Renderer> ().material.mainTexture = oldFace;
		yield return new WaitForSeconds (GetComponent<LevelEvents1_2>().feedDelay);
		speechBubble.SetActive (false);
	}

	public void playClip(GameObject obj , AudioClip clip) {
		obj.GetComponent<AudioSource> ().clip = clip;
		obj.GetComponent<AudioSource> ().Play ();
	}


}
