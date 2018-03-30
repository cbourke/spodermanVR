using System.Collections;
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
//		if (collObj) {
//			if (string.Compare (lastBoxIn, collObj.name, true) != 0) {
//				lastBoxIn = collObj.name;
//				KeyValuePair<int, Texture[]> data;
//				if (LevelBridge.LEVELS.TryGetValue (collObj.name, out data)) {
//					bridgeStatus.open = true;
//					bridgeStatus.newLevel = data.Key;
//					// load textures in window
//					// with data.Value
//				} else {
//					bridgeStatus.open = false;
//					bridgeStatus.newLevel = 0;
//				}
//			}
//		} else {
//			bridgeStatus.open = false;
//			bridgeStatus.newLevel = 0;
//			lastBoxIn = string.Empty;
//			// update to textures 
//			// on the hub object
//		}
//		if (collObj) {
//			switch (collObj.name) {
//			case "LVL1":
//				bridgeStatus.open = true;
//				bridgeStatus.newLevel = 1;
//				break;
//			case "LVL3":
//				bridgeStatus.open = true;
//				bridgeStatus.newLevel = 4;
//				break;
//			case "LVL4":
//				bridgeStatus.open = true;
//				bridgeStatus.newLevel = 5;
//				break;
//			default:
//				bridgeStatus.open = false;
//				break;
//			}
//		} else {
//			bridgeStatus.open = false;
//			bridgeStatus.newLevel = 0;
//		}
	}

	public void OnTriggerEnter(Collider coll) {
		KeyValuePair<int, Texture[]> data;
		if (!LevelBridge.LEVELS.TryGetValue (coll.gameObject.name, out data))
			return;
		
		//collObj = coll.gameObject;
		float ticks = Time.fixedTime;
		
		activeCube = new KeyValuePair<float, GameObject> (ticks, coll.gameObject);
		collidingCubes.Add (coll.gameObject.name, new KeyValuePair<float, GameObject>(ticks, coll.gameObject));
		// texure code
		mainWindowCont.updateArray(data.Value, false);
	}

	public void OnTriggerExit(Collider other) {
		//if (!collObj) {
		//	return;
		//}
		//collObj = null;
		if (!collidingCubes.Remove (other.gameObject.name))
			return;
		
		IEnumerable<KeyValuePair<string, KeyValuePair<float, GameObject>>> order = collidingCubes
			.OrderBy (x => x.Value.Key);

		if (!order.Any()) {
			// nothing on the pad
			Debug.Log("No Cubes On Pad");
			mainWindowCont.updateArray(hub.window1Feed, false);
		} else if (order.First().Value.Key > activeCube.Key) {
			Debug.Log ("Current Item is Newer than one Removed.");
			// current item displayed
			// is more new then the next
			// item in the cube
			// do nothing
		} else {
			Debug.Log ("Render previous Cube.");
			// next needs to be rendered
			// on the window
			activeCube = order.First().Value;
			//Debug.Log (activeCube.Value.name);
			mainWindowCont.updateArray (LevelBridge.LEVELS[activeCube.Value.name].Value , false);
		}
	
	}
}
