using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowTextController : MonoBehaviour {

	public Texture[] textures;
	private SteamVR_TrackedObject leftControllerSteam;
	private SteamVR_TrackedObject rightControllerSteam;
	public GameObject leftCont;
	public GameObject rightCont;
	public int msgLock;
	public int currIndex;

	public EventUtil util;
	public GameObject outline;
	private AudioClip wrong;

	private SteamVR_Controller.Device LeftController {
		get { return SteamVR_Controller.Input ((int)leftControllerSteam.index); }
	}

	private SteamVR_Controller.Device RightController {
		get { return SteamVR_Controller.Input ((int)rightControllerSteam.index); }
	}

	void Awake () {
//		util = GameObject.Find ("WorldNodeTracker").transform.Find("Events").GetComponent<EventUtil>();
		util = EventUtil.FindMe();
		outline = transform.parent.transform.parent.transform.Find("Outline").gameObject;
		leftCont = GameObject.Find ("[CameraRig]").transform.Find ("Controller (left)").gameObject;
		rightCont = GameObject.Find ("[CameraRig]").transform.Find ("Controller (right)").gameObject;
		leftControllerSteam = leftCont.GetComponent<SteamVR_TrackedObject>();
		rightControllerSteam = rightCont.GetComponent<SteamVR_TrackedObject>();
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
