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
	public float hp = 100f;
	public float lastDamTime;

	public static HeadColliderHandler FindMe() {
		return GameObject.FindObjectOfType<HeadColliderHandler>();
	}
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
		Debug.Log (coll.gameObject);
		if (!coll.attachedRigidbody || coll.gameObject.CompareTag("Projectile")) {
			return;
		}
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
		if (hp < 100 && Time.fixedTime - lastDamTime >= 3f) {
			hp += 10f * Time.deltaTime;
			Mathf.Clamp (hp , 0 , 100);
		}
		blur.SetActive (hp < 100);

		if (collObj != null) {
			Damage(50f * Time.deltaTime);
		}
			
		Color tempCol = blur.GetComponent<Renderer> ().material.color;
		tempCol.a = Mathf.Abs(hp - 100f) / 100;
		blur.GetComponent<Renderer> ().material.color = tempCol; 

		if (hp <= 0) {
			Scene loadedLevel = SceneManager.GetActiveScene ();
			SceneManager.LoadScene (loadedLevel.buildIndex);
		}
			
	}

	public void Damage(float dam) {
		lastDamTime = Time.fixedTime;
		hp -= dam;


	}
		
}
