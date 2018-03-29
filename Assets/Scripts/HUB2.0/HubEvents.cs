using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubEvents : MonoBehaviour {

	public GameObject everything;
	public GameObject startZone;
	public bool start;
	private Material skybox;
	public GameObject window1;
	public Texture[] window1Feed;

	private EventUtil util;

	public static HubEvents FindMe() {
		return  GameObject.FindObjectOfType<HubEvents>();
	}

	// Use this for initialization
	void Awake() {
		util = EventUtil.FindMe ();
		util.GetWindowControllerFromWindow(window1).updateArray(window1Feed);
	}
	void Start () {
//		skybox = RenderSettings.skybox;
//		RenderSettings.skybox = null;
		everything.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
//		if (start) {
//			StartHub ();
//		}
	}

	public void StartHub() {
//		RenderSettings.skybox = skybox;
		everything.SetActive (true);
		startZone.SetActive (false);
	}
}
