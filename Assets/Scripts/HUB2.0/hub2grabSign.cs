using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hub2grabSign : MonoBehaviour {

	private EventUtil util;
	public bool active;
	public GameObject levelBridge;

	void Awake() {
		util = EventUtil.FindMe ();
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
//		this.gameObject.SetActive (levelBridge.GetComponent<LevelBridge>().isOpen());
		this.gameObject.GetComponent<SpriteRenderer> ().enabled = levelBridge.GetComponent<LevelBridge> ().isOpen ();
		if (levelBridge.GetComponent<LevelBridge>().isOpen()) 
			LookAt (util.headset);
	}

	private void LookAt(GameObject target) {
		transform.LookAt (util.headset.transform.position - new Vector3(0f , util.headset.transform.position.y - transform.position.y , 0f));
		//		Debug.DrawRay (transform.position , transform.forward , Color.red);
		//		Debug.DrawRay (transform.position , target.transform.position - transform.position , Color.blue);
	}
}
