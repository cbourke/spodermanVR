﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;

public class PadColliderEvents : MonoBehaviour {

	private HubEvents hub;
//	public GameObject collObj;
	public GameObject lvlBridge;
	private LevelBridge bridgeStatus;
	public GameObject lvl1Key;
	public GameObject lvl3Key;
	public GameObject lvl4Key;
	public GameObject mainWindow;
	public WindowTextController mainWindowCont;
	private string lastBoxIn = string.Empty;
	//private Dictionary<float, GameObject> collidingCubes = new Dictionary<float, GameObject>();
	private Dictionary<string, KeyValuePair<float, GameObject>> collidingCubes = new Dictionary<string, KeyValuePair<float, GameObject>>();
	private KeyValuePair<float, GameObject> activeCube;
	private EventUtil util;

	// Use this for initialization
	void Start () {
		util = EventUtil.FindMe ();
		hub = HubEvents.FindMe ();
		bridgeStatus = lvlBridge.GetComponent<LevelBridge> ();
		mainWindowCont = util.GetWindowControllerFromWindow (mainWindow);
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void OnTriggerEnter(Collider coll) {
		KeyValuePair<int, Texture[]> data;
		if (!LevelBridge.LEVELS.TryGetValue (coll.gameObject.name, out data))
			return;
		this.transform.parent.gameObject.GetComponent<AudioSource> ().Play ();
		float ticks = Time.fixedTime;
		
		activeCube = new KeyValuePair<float, GameObject> (ticks, coll.gameObject);
		collidingCubes.Add (coll.gameObject.name, new KeyValuePair<float, GameObject>(ticks, coll.gameObject));
		// texure code
		mainWindowCont.updateArray(data.Value, false);
		bridgeStatus.newLevel = data.Key;
	}

	public void OnTriggerExit(Collider other) {
		//if (!collObj) {
		//	return;
		//}
		//collObj = null;
		if (!collidingCubes.Remove (other.gameObject.name))
			return;
		
		IEnumerable<KeyValuePair<string, KeyValuePair<float, GameObject>>> order = collidingCubes
			.OrderByDescending (x => x.Value.Key);

		if (!order.Any()) {
			// nothing on the pad
			mainWindowCont.updateArray(hub.window1Feed);
			bridgeStatus.newLevel = -1;
		} else if (order.First().Value.Key > activeCube.Key) {
			// current item displayed
			// is more new then the next
			// item in the cube
			// do nothing
		} else {
			// next needs to be rendered
			// on the window
			activeCube = order.First().Value;
			//Debug.Log (activeCube.Value.name);
			mainWindowCont.updateArray (LevelBridge.LEVELS[activeCube.Value.name].Value , false);
			bridgeStatus.newLevel = LevelBridge.LEVELS [activeCube.Value.name].Key;
		}
	
	}
}
