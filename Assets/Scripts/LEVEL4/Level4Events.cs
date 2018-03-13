using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level4Events : MonoBehaviour {

	public GameObject window1;	//intro window
	public GameObject window2;	//first room enter
//	public GameObject window3;	//low health warning
	public Texture[] window1Feed;
	public Texture[] window2Feed;
//	public Texture[] window3Feed;

	private EventUtil util;

	public static Level4Events FindMe() {
		return  GameObject.FindObjectOfType<Level4Events>();
	}
	// Use this for initialization
	void Awake() {
		util = EventUtil.FindMe ();
		util.GetWindowControllerFromWindow (window1).updateArray (window1Feed);
		util.GetWindowControllerFromWindow (window2).updateArray (window2Feed);
//		util.GetWindowControllerFromWindow (window3).updateArray (window3Feed);
	}
	void Start () {
		window2.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TriggerZone1() {
		window2.SetActive (true);
		util.GetAnimFromWindow (window1).SetTrigger ("TurnOff");
	}
}
