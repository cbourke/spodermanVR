using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadColliderEvents : MonoBehaviour {

	private HubEvents hub;
	public GameObject collObj;
	public GameObject lvlBridge;
	private LevelBridge bridgeStatus;
	public GameObject lvl1Key;
	public GameObject lvl3Key;
	public GameObject lvl4Key;
	// Use this for initialization
	void Start () {
		hub = HubEvents.FindMe ();
		bridgeStatus = lvlBridge.GetComponent<LevelBridge> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (collObj) {
			switch (collObj.name) {
			case "LVL1":
				bridgeStatus.open = true;
				bridgeStatus.newLevel = 1;
				break;
			case "LVL3":
				bridgeStatus.open = true;
				bridgeStatus.newLevel = 4;
				break;
			case "LVL4":
				bridgeStatus.open = true;
				bridgeStatus.newLevel = 5;
				break;
			default:
				bridgeStatus.open = false;
				break;
			}
		} else {
			bridgeStatus.open = false;
			bridgeStatus.newLevel = 0;
		}
	}

	public void OnTriggerEnter(Collider coll) {
		collObj = coll.gameObject;
	}

	public void OnTriggerExit(Collider other) {
		if (!collObj) {
			return;
		}
		collObj = null;
	}
}
