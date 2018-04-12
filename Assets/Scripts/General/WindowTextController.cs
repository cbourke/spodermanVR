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
	public float lastTexChange = 0;

	public EventUtil util;
	public GameObject outline;
	private AudioClip wrong;
	public GameObject leftPrevArr;
	public GameObject rightPrevArr;
	private Texture originalTex;

	private SteamVR_Controller.Device LeftController {
		get { return SteamVR_Controller.Input ((int)leftControllerSteam.index); }
	}

	private SteamVR_Controller.Device RightController {
		get { return SteamVR_Controller.Input ((int)rightControllerSteam.index); }
	}
		

	void Awake () {
		util = EventUtil.FindMe();
//		outline = transform.parent.transform.parent.transform.Find("Outline").gameObject;
		leftCont = GameObject.Find ("[CameraRig]").transform.Find ("Controller (left)").gameObject;
		rightCont = GameObject.Find ("[CameraRig]").transform.Find ("Controller (right)").gameObject;
		leftControllerSteam = leftCont.GetComponent<SteamVR_TrackedObject>();
		rightControllerSteam = rightCont.GetComponent<SteamVR_TrackedObject>();
		msgLock = int.MaxValue;
		wrong = (AudioClip)Resources.Load ("Audio/windowAudio/error");
		originalTex = GetComponent<Renderer> ().material.mainTexture;
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
					lastTexChange = Time.fixedTime;
				}
			}
			else if (RightController.GetPressDown(SteamVR_Controller.ButtonMask.Grip)) {
				if (currIndex >= msgLock || currIndex >= textures.GetLength (0) - 1) {
					util.playClip (this.gameObject.transform.parent.gameObject , wrong);
				} else {
					util.changeTex (this.transform.parent.gameObject , this.gameObject , textures[++currIndex]);
					lastTexChange = Time.fixedTime;
				}
			}
		} else {
			outline.SetActive (false);
		}
//		leftPrevArr.GetComponent<SpriteRenderer>().SetActive (Time.fixedTime - lastTexChange >= 3.0f && currIndex != 0);
		leftPrevArr.GetComponent<SpriteRenderer>().enabled = Time.fixedTime - lastTexChange >= 3.0f && currIndex != 0;
//		rightPrevArr.SetActive (Time.fixedTime - lastTexChange >= 3.0f && currIndex < msgLock && currIndex < textures.GetLength(0));
		rightPrevArr.GetComponent<SpriteRenderer>().enabled = Time.fixedTime - lastTexChange >= 3.0f && currIndex < msgLock && currIndex < textures.GetLength(0) - 1;
	}

	public void ChangeMsg(int msgInd) {
		currIndex = msgInd;
		util.changeTex (this.transform.parent.gameObject , this.gameObject , textures[msgInd]);
		lastTexChange = Time.fixedTime;
	}

	public void updateArray() {
		updateArray (new Texture[0]);
	}

	public void updateArray (Texture[] tex, bool includeFirstTexture = true) {
		List<Texture> tempTex = new List<Texture> (tex);
		if (includeFirstTexture)
			tempTex.Insert (0, originalTex);
		else {
			GetComponent<Renderer>().material.mainTexture = tempTex[0];     //////bad??

		}
		this.textures = tempTex.ToArray ();
		currIndex = 0;
		msgLock = int.MaxValue;
		GetComponent<Renderer>().material.mainTexture = tempTex[0];			////bad2??
	}

	public void ChangeLock (int msgInd) {
		if (msgInd > textures.GetLength(0) - 1) return;
		msgLock = msgInd;
	}
		
}
