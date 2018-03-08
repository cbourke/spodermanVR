using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubEvents : MonoBehaviour {

	public GameObject everything;
	public bool start;
	private Material skybox;

	public static HubEvents FindMe() {
		return  GameObject.FindObjectOfType<HubEvents>();
	}

	// Use this for initialization
	void Start () {
		skybox = RenderSettings.skybox;
		RenderSettings.skybox = null;
		everything.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (start) {
			StartHub ();
		}
	}

	public void StartHub() {
		RenderSettings.skybox = skybox;
		everything.SetActive (true);
	}
}
