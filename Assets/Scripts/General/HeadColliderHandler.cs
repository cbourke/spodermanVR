using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeadColliderHandler : MonoBehaviour {

	public GameObject blur;
	public float blurSpeed;
	private int headLayer;
	private int cameraZoneLayer;
	public GameObject collObj;
	private GameObject world;
	private Color startingBlurCol;
	// Use this for initialization
	void Awake() {
		headLayer = LayerMask.NameToLayer ("Default");
		cameraZoneLayer = LayerMask.NameToLayer ("CameraZoneCollisions");
		Physics.IgnoreLayerCollision (headLayer , cameraZoneLayer , true);
		world = GameObject.Find ("WorldNodeTracker");
		blur.SetActive (false);
		startingBlurCol = blur.GetComponent<Renderer> ().material.color;
	}

	void Start () {

	}

	public void OnTriggerEnter (Collider coll) {
		SetCollidingObject (coll.attachedRigidbody.gameObject);
	}

	public void OnTriggerStay(Collider coll) {

	}

	public void OnTriggerExit (Collider coll) {
		if (!collObj) {
			return;
		}
		collObj = null;
	}

	private void SetCollidingObject (GameObject obj) {
		if (collObj || !obj.GetComponent<Rigidbody>()) {
			return;
		}
		collObj = obj;
	}



	// Update is called once per frame
	void Update () {
		if (collObj != null) {
			blur.SetActive (true);
			Color tempCol = blur.GetComponent<Renderer> ().material.color;
			tempCol.a += blurSpeed * Time.deltaTime;
			blur.GetComponent<Renderer> ().material.color = tempCol;
			if (tempCol.a >= 255) {
				Scene loadedLevel = SceneManager.GetActiveScene ();
				SceneManager.LoadScene (loadedLevel.buildIndex);
			}
		}
		else  {
			//incomplete
//			blur.GetComponent<Renderer> ().material.color = startingBlurCol;
//			blur.SetActive (false);
			if (blur.activeSelf) {
				Color tempCol = blur.GetComponent<Renderer> ().material.color;
				tempCol.a -= blurSpeed * Time.deltaTime;
				blur.GetComponent<Renderer> ().material.color = tempCol;
				if (tempCol.a <= 1) {
					blur.SetActive (false);
				}
			}

		}
	}
}
