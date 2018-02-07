using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowTextController : MonoBehaviour {

	public Texture[] textures;
	public SteamVR_TrackedObject leftController;
	public SteamVR_TrackedObject rightController;
	public int msgLock;
	public int currIndex;

	public EventUtil util;
	public GameObject outline;
	private AudioClip wrong;

	private SteamVR_Controller.Device LeftController {
		get { return SteamVR_Controller.Input ((int)leftController.index); }
	}

	private SteamVR_Controller.Device RightController {
		get { return SteamVR_Controller.Input ((int)rightController.index); }
	}

	void Awake () {
		util = GameObject.Find ("WorldNodeTracker").transform.Find("Events").GetComponent<EventUtil>();
		outline = transform.parent.transform.parent.transform.Find("Outline").gameObject;
		leftController = GameObject.Find ("[CameraRig]").transform.Find ("Controller (left)").GetComponent<SteamVR_TrackedObject>();
		rightController = GameObject.Find ("[CameraRig]").transform.Find ("Controller (right)").GetComponent<SteamVR_TrackedObject>();
		msgLock = int.MaxValue;
		wrong = (AudioClip)Resources.Load ("Audio/windowAudio/error");
	}
	// Use this for initialization
	void Start () {
		outline.SetActive (false);

	}
	
	// Update is called once per frame
	void Update () {
		if (textures.GetLength(0) <= 0) {
			return;
		}
		if (util.visibleObj != null && util.visibleObj.GetInstanceID () == this.gameObject.GetInstanceID ()) {
			outline.SetActive (true);
			if (LeftController.GetPressDown(SteamVR_Controller.ButtonMask.Grip)) {
				if (currIndex == 0) {
					util.playClip (this.gameObject.transform.parent.gameObject , wrong);
				} else {
					util.changeTex (this.transform.parent.gameObject , this.gameObject , textures[--currIndex]);
				}
			}
			else if (RightController.GetPressDown(SteamVR_Controller.ButtonMask.Grip)) {
				if (currIndex >= msgLock || currIndex >= textures.GetLength (0) - 1) {
					util.playClip (this.gameObject.transform.parent.gameObject , wrong);
				} else {
					util.changeTex (this.transform.parent.gameObject , this.gameObject , textures[++currIndex]);
				}
			}
		} else {
			outline.SetActive (false);
		}
	}

	public void updateArray (Texture[] tex) {
		Texture firstTex = GetComponent<Renderer> ().material.mainTexture;
		List<Texture> tempTex = new List<Texture> (tex);
		tempTex.Insert (0, firstTex);
		this.textures = tempTex.ToArray ();
	}

	public IEnumerator continuePrompt () {
		yield return new WaitForSeconds (2.0f);

	}
}
